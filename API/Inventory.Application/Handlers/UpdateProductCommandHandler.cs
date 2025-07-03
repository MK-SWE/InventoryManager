using Inventory.Application.Commands;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers;

public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IWriteRepository<Product> _productRepository;

    public UpdateProductCommandHandler(IWriteRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _productRepository.UpdateByIdAsync(request.Id, (product) =>
            {
                // Map properties from DTO to domain entity
                product = product with { ProductName = request.UpdateProduct.ProductName ?? product.ProductName };
                product = product with { ProductDescription = request.UpdateProduct.ProductDescription ?? product.ProductDescription };
                product = product with { CategoryId = request.UpdateProduct.CategoryId ?? product.CategoryId };
                product = product with { UnitOfMeasureId = request.UpdateProduct.UnitOfMeasureId ?? product.UnitOfMeasureId };
                product = product with { UnitPrice = request.UpdateProduct.UnitPrice ?? product.UnitPrice };
                product = product with { ReorderLevel = request.UpdateProduct.ReorderLevel ?? product.ReorderLevel };
                product = product with { Weight = request.UpdateProduct.Weight ?? product.Weight };
                product = product with { Volume = request.UpdateProduct.Volume ?? product.Volume };
                product = product with { IsActive = request.UpdateProduct.IsActive ?? product.IsActive };
            });
        }
        catch (KeyNotFoundException ex)
        {
            throw new NotFoundException($"Product {request.Id} not found", ex);
        }
    }
}