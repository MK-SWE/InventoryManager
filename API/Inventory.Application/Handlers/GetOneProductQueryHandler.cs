using Inventory.Application.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers;

public class GetOneProductQueryHandler: IRequestHandler<GetOneProductQuery, Product?>
{
    private readonly IReadRepository<Product> _productsRepository;

    public GetOneProductQueryHandler(IReadRepository<Product> productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<Product?> Handle(GetOneProductQuery request, CancellationToken cancellationToken)
    {
        return await _productsRepository.GetById(request.Id);
    }
}