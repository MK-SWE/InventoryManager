using Inventory.Application.Common.Exceptions;
using Inventory.Application.InventoryStock.Queries;
using Inventory.Application.Products.Queries;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class GetProductWithStockHandler : IRequestHandler<GetProductWithStockQuery, ProductWithStocksResponseDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductWithStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductWithStocksResponseDto> Handle(GetProductWithStockQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithStocksAsync(request.ProductId, cancellationToken);
        if (product is not null)
            return product;
        throw new NotFoundException(name: "Product", key: request.ProductId);
    }
}