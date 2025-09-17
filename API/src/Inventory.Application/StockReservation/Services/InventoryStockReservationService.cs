using Inventory.Application.Common.Interfaces;
using Inventory.Application.StockReservation.DTOs;
using Inventory.Application.StockReservation.ValueObjects;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.StockReservation.Services;

public class InventoryStockReservationService :IInventoryStockReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductStockRepository _productStockRepository;
    private readonly IProductService _productService;
    private readonly IInventoryStockReservationRepository _reservationRepository;

    public InventoryStockReservationService( IUnitOfWork unitOfWork, IProductStockRepository productStockRepository, IProductService productService, IInventoryStockReservationRepository reservationRepository)
    {
        _unitOfWork = unitOfWork;
        _productStockRepository = productStockRepository;
        _productService = productService;
        _reservationRepository = reservationRepository;
    }
    
    public async Task<IReservationResult> ReserveStockAsync(IReadOnlyCollection<CreateReservationLineCommandDto> reservationLines, CancellationToken ct = default)
    {
        if (reservationLines == null || !reservationLines.Any())
            throw new ArgumentException("Reserve items collection cannot be null or empty.", nameof(reservationLines));
        HashSet<Product> products = await _productService.GetProductsByIds(reservationLines.Select(ri => ri.ProductId).ToHashSet());
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var newReservation = InventoryStockReservation.Create(DateTime.UtcNow + TimeSpan.FromDays(7));
            foreach (var reservationLine in reservationLines)
            {
                Product? product = products.FirstOrDefault(p => p.Id == reservationLine.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {reservationLine.ProductId} not found.");
                if (!product.CanReserve(reservationLine.Quantity))
                    throw new InvalidOperationException($"Insufficient stock for product ID {reservationLine.ProductId}. Requested: {reservationLine.Quantity}, Available: {product.TotalAvailableStock}");
                newReservation.AddReservationLine(reservationLine.ProductId, reservationLine.Quantity, allocatedQuantity: 0);
            }
            
            await _reservationRepository.AddAsync(newReservation, ct);
            await _unitOfWork.CommitAsync(ct);
            return new ReserveStockReference(
                reserveId: newReservation.ReservationReference,
                isSuccess: true,
                expireDate: newReservation.ExpiresAt,
                errorMessage: null);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            return new ReservationOperationResult(
                isSuccess: false,
                errorMessage: ex.ToString());
        }
    }

    public async Task<ReservationOperationResult> AllocateReservationAsync(Guid reference, StockReservationAllocationDto stockReservationAllocationDto, CancellationToken ct = default)
{
    if (!stockReservationAllocationDto.AllocateItems.Any())
        return new ReservationOperationResult(false, "Allocation DTO or its allocate items cannot be null or empty.");
    
    var reservation = await _reservationRepository.GetReservationWithLinesAsync(reference, ct);
    if (reservation == null)
        return new ReservationOperationResult(false, $"Reservation with reference {reference} was not found.");
    
    HashSet<int> productIds = stockReservationAllocationDto.AllocateItems.Select(items => items.ProductId).ToHashSet();
    HashSet<int> warehouseIds = stockReservationAllocationDto.AllocateItems
                .SelectMany(item => item.WarehouseAllocations)
                .Select(wa => wa.WarehouseId)
                .ToHashSet();

    HashSet<ProductStock> productStocks = await _productService.GetProductsStocksInWarehouses(productIds, warehouseIds);
    Dictionary<(int ProductId, int WarehouseId), ProductStock> productStockDict = productStocks.ToDictionary(ps => (ps.ProductId, ps.WarehouseId));

    List<ProductStock> updatedStocks = new List<ProductStock>();
    
    await _unitOfWork.BeginTransactionAsync(ct);
    try
    {
        foreach (var productAllocationDto in stockReservationAllocationDto.AllocateItems)
        {
            var reservationLine = reservation.ReservationLines.FirstOrDefault(line => line.ProductId == productAllocationDto.ProductId);
            if (reservationLine == null)
                return new ReservationOperationResult(false, $"Reservation line for Product ID {productAllocationDto.ProductId} not found in reservation.");
            
            foreach (var warehouseAllocationDto in productAllocationDto.WarehouseAllocations)
            {
                var key = (productAllocationDto.ProductId, warehouseAllocationDto.WarehouseId);
                
                if (!productStockDict.TryGetValue(key, out var productStock))
                {
                    return new ReservationOperationResult(false, 
                        $"Product stock for Product ID {productAllocationDto.ProductId} in Warehouse ID {warehouseAllocationDto.WarehouseId} not found.");
                }
                
                if (productStock.StockStatus.AvailableStock < warehouseAllocationDto.AllocatedQuantity)
                {
                    return new ReservationOperationResult(false, 
                        $"Insufficient stock for Product ID {productAllocationDto.ProductId} in Warehouse ID {warehouseAllocationDto.WarehouseId}. " +
                        $"Requested: {warehouseAllocationDto.AllocatedQuantity}, Available: {productStock.StockStatus.AvailableStock}");
                }
                
                productStock.AllocateStock(warehouseAllocationDto.AllocatedQuantity);
                updatedStocks.Add(productStock);
                reservationLine.AllocatedQuantity += warehouseAllocationDto.AllocatedQuantity;
            }
        }
        
        if (updatedStocks.Any())
        {
            await _productStockRepository.UpdateRangeAsync(updatedStocks, ct);
        }
        
        await _reservationRepository.UpdateAsync(reservation, ct);
        await _unitOfWork.CommitAsync(ct);
        return new ReservationOperationResult(true);
    }
    catch (Exception ex)
    {
        await _unitOfWork.RollbackAsync(ct);
        return new ReservationOperationResult(false, ex.Message);
    }
    finally
    {
        _unitOfWork.Dispose();
    }
}
    
    public async Task<ReservationOperationResult> UpdateReservationAsync( Guid reference, UpdateStockReservationCommandDto updateDto, CancellationToken ct = default)
{
    if (updateDto.ReservationLines == null || !updateDto.ReservationLines.Any())
        return new ReservationOperationResult(false, "Update DTO or its reservation lines cannot be null or empty.");
    
    var reservation = await _reservationRepository.GetReservationWithLinesAsync(reference, ct);
    if (reservation == null)
        return new ReservationOperationResult(false, $"Reservation with reference {reference} was not found.");
    
    bool hasChanges = false;
    var existingProductIds = new HashSet<int>();
    
    foreach (var line in updateDto.ReservationLines)
    {
        var existingLine = reservation.ReservationLines.FirstOrDefault(l => l.ProductId == line.ProductId);
        if (existingLine != null)
        {
            if (line.Quantity != existingLine.ReservedQuantity)
            {
                existingLine.ReservedQuantity = line.Quantity;
                hasChanges = true;
            }
            existingProductIds.Add(line.ProductId);
        }
        else
        {
            reservation.AddReservationLine(
                productId: line.ProductId,
                reservedQuantity: line.Quantity
            );
            hasChanges = true;
            existingProductIds.Add(line.ProductId);
        }
    }

    // Remove lines not included in the update (if required by business logic)
    var linesToRemove = reservation.ReservationLines
        .Where(l => !existingProductIds.Contains(l.ProductId))
        .ToList();
    
    foreach (var line in linesToRemove)
    {
        reservation.ReservationLines.Remove(line);
        hasChanges = true;
    }

    if (hasChanges)
    {
        await _reservationRepository.UpdateAsync(reservation, ct);
    }

    return new ReservationOperationResult(true);
}

    public async Task<ReservationOperationResult> CancelReservationAsync(Guid reference, CancellationToken ct = default)
    {
        var reservation = await _reservationRepository.GetByReferenceWithDetailsAsync(reference, ct);
        if (reservation == null || reservation.IsDeleted)
        {
            // Reservation not found or Reservation already cancelled, nothing to cancel
            return new ReservationOperationResult(
                isSuccess: false,
                errorMessage: $"Reservation with reference {ct} not found or already cancelled."
            );
        }
        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            await _reservationRepository.HardDeleteAsync(reservation.Id, ct);
            await _unitOfWork.CommitAsync(ct);
            return new ReservationOperationResult(isSuccess: true, errorMessage: null);
        }
        catch(Exception ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            return new ReservationOperationResult(isSuccess: false, errorMessage: ex.ToString());
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }
}