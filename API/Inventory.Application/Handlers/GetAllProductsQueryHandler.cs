using Inventory.Application.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers;

public class GetAllProductsQueryHandler: IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IReadRepository<Product> _productsRepository;

    public GetAllProductsQueryHandler(IReadRepository<Product> productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productsRepository.GetAllAsync();
        return products;
    }
}