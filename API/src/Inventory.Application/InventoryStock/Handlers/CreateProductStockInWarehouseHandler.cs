using Inventory.Application.InventoryStock.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.InventoryStock.Handlers;

public class CreateProductStockInWarehouseHandler 
    : IRequestHandler<CreateProductStockInWarehouseCommand, int>
{
    private readonly IProductStockRepository _productStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductStockInWarehouseHandler(
        IProductStockRepository productStockRepository,
        IUnitOfWork unitOfWork)
    {
        _productStockRepository = productStockRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(
        CreateProductStockInWarehouseCommand request, 
        CancellationToken cancellationToken)
    {
        var productId = request.CreateProductStockInWarehouseDto.ProductId;
        var warehouseId = request.CreateProductStockInWarehouseDto.WarehouseId;
        var initialQuantity = request.CreateProductStockInWarehouseDto.Amount;
        
        var productStock = ProductStock.Create(productId, warehouseId);
        productStock.AddStock(initialQuantity);
        await _productStockRepository.AddAsync(productStock, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return productStock.Id;
    }
}