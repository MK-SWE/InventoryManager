using Inventory.Application.Products.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class DeleteProductCommandHandler: IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (!await _productRepository.ExistsAsync(request.Id, cancellationToken)) 
            throw new NotFoundException($"Product with id: {request.Id}", "not found");
        try
        {
            await _productRepository.DeleteAsync(request.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"{ex}");
        }
    }
}