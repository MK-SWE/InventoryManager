using Inventory.Application.Products.Commands;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Domain.ValueObjects.Products;
using MediatR;

namespace Inventory.Application.Products.Handlers;

public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateProductCommandDto;
    
        var identificationParams = new ProductIdentificationCreationParams(
            dto.SKU,
            dto.ProductName,
            dto.ProductDescription,
            dto.CategoryId,
            dto.UnitOfMeasureId,
            dto.ReorderLevel
        );

        var dimensionsParams = new ProductDimensionsCreationParams(
            dto.Weight,
            dto.Volume
        );

        var pricesParams = new ProductPricesCreationParams(
            dto.UnitCost,
            dto.UnitPrice
        );

        Product product = Product.Create(
            identificationParams,
            dimensionsParams,
            pricesParams
        );
    
        return await _productRepository.AddAsync(product, cancellationToken);
    }
}