using Inventory.Application.Common.Exceptions;
using Inventory.Application.Products.Queries;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Shared.DTOs.Products;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class GetProductQueryHandler: IRequestHandler<GetProductQuery, GetProductsResponseDto?>
{
    private readonly IProductRepository _productsRepository;

    public GetProductQueryHandler(IProductRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<GetProductsResponseDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _productsRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Product {request.Id} not found", ex);
        }
    }
}