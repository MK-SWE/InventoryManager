using Inventory.Application.Common.Exceptions;
using Inventory.Application.InventoryStock.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.InventoryStock.Handlers;

public class TransferStockBetweenWarehousesCommandHandler : IRequestHandler<TransferStockBetweenWarehousesCommand>
{
    private readonly IProductStockRepository _productStockRepository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferStockBetweenWarehousesCommandHandler(IProductStockRepository productStockRepository,
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _productStockRepository = productStockRepository;
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(TransferStockBetweenWarehousesCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var sourceProductStock = await _productStockRepository.GetByProductAndWarehouseAsync(
                request.ProductId,
                request.TransferProductStockDto.SourceWarehouseId, 
                cancellationToken);
            
            var destinationProductStock = await _productStockRepository.GetByProductAndWarehouseAsync(
                request.ProductId,
                request.TransferProductStockDto.DestinationWarehouseId, 
                cancellationToken);

            if (sourceProductStock is null) 
                throw new InvalidStockOperationException("Transfer", $"Product {request.ProductId} has no stock in source warehouse");
            
            if (sourceProductStock.Quantity < request.TransferProductStockDto.Quantity)
                throw new InvalidStockOperationException("Transfer", "Insufficient quantity in source warehouse");
            
            sourceProductStock.RemoveStock(request.TransferProductStockDto.Quantity);
            
            if (destinationProductStock is null)
            {
                destinationProductStock = ProductStock.Create(
                    request.ProductId,
                    request.TransferProductStockDto.DestinationWarehouseId,
                    request.TransferProductStockDto.Quantity);
                await _productStockRepository.AddAsync(destinationProductStock, cancellationToken);
            }
            else
            {
                destinationProductStock.AddStock(request.TransferProductStockDto.Quantity);
                await _productStockRepository.UpdateAsync(destinationProductStock, cancellationToken);
            }
            
            await _productStockRepository.UpdateAsync(sourceProductStock, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw new InvalidStockOperationException("Cannot transfer stock", exception.Message);
        }
    }
}
