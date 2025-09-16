using Inventory.Application.Common.Exceptions;
using Inventory.Application.InventoryStock.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.InventoryStock.Handlers;

public class GetStockByProductQueryHandler: IRequestHandler<GetStockByProductQuery, IReadOnlyList<ProductStock>>
{
    private readonly IProductStockRepository _productStockRepository;

    public GetStockByProductQueryHandler(IProductStockRepository productStockRepository)
    {
        _productStockRepository = productStockRepository;
    }
    

    public async Task<IReadOnlyList<ProductStock>> Handle(GetStockByProductQuery request, CancellationToken cancellationToken)
    {
        var productStock = await _productStockRepository.GetByProductAsync(request.ProductId, cancellationToken);
        if (productStock.Count == 0)
        {
            throw new NotFoundException("GetProductStock", $"Stock for product {request.ProductId} not found");
        }
        return productStock;
    }
}