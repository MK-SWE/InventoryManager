using Inventory.Application.Products.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class DeleteProductCommandHandler: IRequestHandler<DeleteProductCommand>
{
    private readonly IWriteRepository<Product> _productRepository;

    public DeleteProductCommandHandler(IWriteRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _productRepository.DeleteByIdAsync(request.Id);
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Product {request.Id} not found", ex);
        }
    }
}