using Inventory.Application.InventoryStock.Commands;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.InventoryStock.Handlers;

public class RemoveProductStockFromWarehouseCommandHandler: IRequestHandler<RemoveProductStockFromWarehouseCommand>
{
    private readonly IProductStockRepository _productStockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProductStockFromWarehouseCommandHandler(IProductStockRepository productStockRepository, IUnitOfWork unitOfWork)
    {
        _productStockRepository = productStockRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(RemoveProductStockFromWarehouseCommand request, CancellationToken cancellationToken)
    {
        var productStock = await _productStockRepository.GetByProductAndWarehouseAsync(request.ProductId, request.WarehouseId, cancellationToken);
        if (productStock is not null)
        {
            await _productStockRepository.DeleteAsync(productStock, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}