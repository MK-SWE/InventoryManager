using Inventory.Application.Common.Exceptions;
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
        try
        {
            return await _productsRepository.GetByIdAsync(request.Id);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Product {request.Id} not found", ex);
        }
    }
}