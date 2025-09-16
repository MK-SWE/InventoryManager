using Inventory.Application.Products.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs.Products;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class GetAllProductsQueryHandler: IRequestHandler<GetAllProductsQuery, (IReadOnlyList<GetProductsResponseDto> Items, int TotalCount)>
{
    private readonly IProductRepository _productsRepository;

    public GetAllProductsQueryHandler(IProductRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<(IReadOnlyList<GetProductsResponseDto> Items, int TotalCount)> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        (IReadOnlyList<GetProductsResponseDto> Items, int TotalCount) products = await _productsRepository.GetPagedAsync(request.pageNumber, request.PageSize, cancellationToken);
        return products;
    }
}