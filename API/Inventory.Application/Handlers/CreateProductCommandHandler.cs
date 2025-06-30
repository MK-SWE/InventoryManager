using Inventory.Application.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Handlers;

public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand, Product>
{
    private readonly IWriteRepository<Product> _productRepository;

    public CreateProductCommandHandler(IWriteRepository<Product> ProductRepository)
    {
        _productRepository = ProductRepository;
    }
    
    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return await _productRepository.CreateNew(() => 
            new Product 
            {
                SKU = request.NewProductDTO.SKU,
                ProductName = request.NewProductDTO.ProductName,
                ProductDescription = request.NewProductDTO.ProductDescription,
                CategoryId = request.NewProductDTO.CategoryId,
                UnitOfMeasureId = request.NewProductDTO.UnitOfMeasureId,
                UnitPrice = request.NewProductDTO.UnitPrice,
                ReorderLevel = request.NewProductDTO.ReorderLevel,
                Weight = request.NewProductDTO.Weight,
                Volume = request.NewProductDTO.Volume,
                IsActive = request.NewProductDTO.IsActive
            });
    }
}