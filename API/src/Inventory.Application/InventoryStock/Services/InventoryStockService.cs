using Inventory.Application.Common.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.InventoryStock.Services;

public class InventoryStockService: IInventoryStockService
{
    private readonly IProductStockRepository _stockRepo;
    
    public InventoryStockService(IProductStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }
    
    public async Task ReceiveStock(InventoryTransaction transaction, CancellationToken cancellationToken)
    {
        if (!transaction.DestinationWarehouseId.HasValue)
            return;
        
        var (stockLookup, productIds) = await PrepareStockData(transaction, cancellationToken);
        if (productIds.Count == 0) return;

        var newStocks = new Dictionary<(int, int), ProductStock>();
        var updatedStocks = new HashSet<ProductStock>();

        foreach (var line in transaction.Lines)
        {
            var key = (line.ProductId, transaction.DestinationWarehouseId.Value);
            var existingStock = stockLookup[key].FirstOrDefault();

            if (existingStock is null)
            {
                if (!newStocks.TryGetValue(key, out var newStock))
                {
                    newStock = ProductStock.Create(line.ProductId, transaction.DestinationWarehouseId.Value, 0);
                    newStocks[key] = newStock;
                }
            }
            else
            {
                existingStock.AddStock(line.Quantity);
                updatedStocks.Add(existingStock);
            }
        }

        if (newStocks.Count > 0)
            await _stockRepo.AddRangeAsync(newStocks.Values, cancellationToken);

        if (updatedStocks.Count > 0)
            await _stockRepo.UpdateRangeAsync(updatedStocks, cancellationToken);
    }

    public async Task ShipStock(InventoryTransaction transaction, CancellationToken cancellationToken)
    {
        if (!transaction.SourceWarehouseId.HasValue)
            return;

        var (stockLookup, productIds) = await PrepareStockData(transaction, cancellationToken);
        if (productIds.Count == 0) return;

        var updatedStocks = new HashSet<ProductStock>();

        foreach (var line in transaction.Lines)
        {
            var key = (line.ProductId, transaction.SourceWarehouseId.Value);
            var stock = stockLookup[key].FirstOrDefault();

            ValidateStockAvailability(
                stock,
                line.Quantity,
                transaction.TransactionType.ToString(),
                line.ProductId,
                transaction.SourceWarehouseId.Value
            );

            stock!.ShipStock(line.Quantity);
            updatedStocks.Add(stock);
        }

        if (updatedStocks.Count > 0)
            await _stockRepo.UpdateRangeAsync(updatedStocks, cancellationToken);
    }

    public async Task StockTransfer(InventoryTransaction transaction, CancellationToken cancellationToken)
    {
        if (!transaction.SourceWarehouseId.HasValue || !transaction.DestinationWarehouseId.HasValue)
            throw new InvalidOperationException("Both source and destination warehouses are required for transfers");
        
        var (stockLookup, productIds) = await PrepareStockData(transaction, cancellationToken);
        if (productIds.Count == 0) return;

        var newStocks = new Dictionary<(int, int), ProductStock>();
        var updatedStocks = new HashSet<ProductStock>();

        foreach (var line in transaction.Lines)
        {
            // Process source
            var sourceKey = (line.ProductId, transaction.SourceWarehouseId.Value);
            var sourceStock = stockLookup[sourceKey].FirstOrDefault();

            ValidateStockAvailability(
                sourceStock,
                line.Quantity,
                transaction.TransactionType.ToString(),
                line.ProductId,
                transaction.SourceWarehouseId.Value
            );

            // Process destination
            var destKey = (line.ProductId, transaction.DestinationWarehouseId.Value);
            var destStock = stockLookup[destKey].FirstOrDefault();

            if (destStock is null)
            {
                if (!newStocks.TryGetValue(destKey, out var newStock))
                {
                    newStock = ProductStock.Create(line.ProductId, transaction.DestinationWarehouseId.Value, 0);
                    newStocks[destKey] = newStock;
                }
                destStock = newStock;
            }
            else
            {
                updatedStocks.Add(destStock);
            }

            // Perform transfer
            sourceStock!.RemoveStock(line.Quantity);
            destStock.AddStock(line.Quantity);

            // Track source changes
            updatedStocks.Add(sourceStock);
        }

        if (newStocks.Count > 0)
            await _stockRepo.AddRangeAsync(newStocks.Values, cancellationToken);

        if (updatedStocks.Count > 0)
            await _stockRepo.UpdateRangeAsync(updatedStocks, cancellationToken);
    }
    
    public async Task AdjustStock(InventoryTransaction transaction, CancellationToken cancellationToken)
    {
        if (!transaction.DestinationWarehouseId.HasValue)
            throw new InvalidOperationException("Destination warehouse is required for stock adjustments");
        
        var (stockLookup, productIds) = await PrepareStockData(transaction, cancellationToken);
        if (productIds.Count == 0) return;

        var newStocks = new Dictionary<(int, int), ProductStock>();
        var updatedStocks = new HashSet<ProductStock>();
        var warehouseId = transaction.DestinationWarehouseId;

        foreach (var line in transaction.Lines)
        {
            var key = (line.ProductId, warehouseId.Value);
            var stock = stockLookup[key].FirstOrDefault();

            if (stock is null)
            {
                if (!newStocks.TryGetValue(key, out var newStock))
                {
                    newStock = ProductStock.Create(line.ProductId, warehouseId.Value, 0);
                    newStocks[key] = newStock;
                }
                stock = newStock;
            }
            else
            {
                updatedStocks.Add(stock);
            }

            // Apply adjustment
            stock.AdjustStock(line.Quantity);
        }

        if (newStocks.Count > 0)
            await _stockRepo.AddRangeAsync(newStocks.Values, cancellationToken);

        if (updatedStocks.Count > 0)
            await _stockRepo.UpdateRangeAsync(updatedStocks, cancellationToken);
    }

    #region Helper Methods

    private async Task<(
            ILookup<(int ProductId, int WarehouseId), ProductStock> stockLookup, 
            HashSet<int> productIds)>
        PrepareStockData(InventoryTransaction transaction, CancellationToken cancellationToken)
    {
        var productIds = transaction.Lines
            .Select(l => l.ProductId)
            .Distinct()
            .ToHashSet();

        if (productIds.Count == 0)
            return (Enumerable.Empty<ProductStock>().ToLookup(s => (s.ProductId, s.WarehouseId)), productIds);

        var allStocks = await _stockRepo.GetByProductsIdsAsync(productIds.ToArray(), cancellationToken);
        return (allStocks.ToLookup(s => (s.ProductId, s.WarehouseId)), productIds);
    }

    private static void ValidateStockAvailability(
        ProductStock? stock,
        decimal requiredQuantity,
        string operation,
        int productId,
        int warehouseId)
    {
        if (stock is null)
            throw new InvalidStockOperationException(operation,
                $"Product {productId} not found in warehouse {warehouseId}");

        if (stock.Quantity < requiredQuantity)
            throw new InvalidStockOperationException(operation,
                $"Insufficient stock for product {productId} (Available: {stock.Quantity}, Required: {requiredQuantity})");
    }

    #endregion
}