using Inventory.Application.InventoryStock.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.InventoryStock.Handlers;

public class UpdateProductStockInWarehouseCommandHandler: IRequestHandler<UpdateProductStockInWarehouseCommand>
{
    private readonly IProductStockRepository _productStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductStockInWarehouseCommandHandler(IProductStockRepository productStockRepository, IUnitOfWork unitOfWork)
    {
        _productStockRepository = productStockRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UpdateProductStockInWarehouseCommand request, CancellationToken cancellationToken)
    {
        ProductStock? productStock = await _productStockRepository.GetByProductAndWarehouseAsync(request.ProductId, request.WarehouseId, cancellationToken);
        if (productStock is not null)
        {
            productStock.AdjustStock(request.Amount);
            await _productStockRepository.UpdateAsync(productStock, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}