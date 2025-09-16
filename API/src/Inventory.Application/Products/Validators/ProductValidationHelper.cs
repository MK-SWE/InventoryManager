using FluentValidation;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.Products.Validators;

public class ProductValidationHelper
{
    private readonly IProductRepository _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;

    public ProductValidationHelper(
        IProductRepository productRepository,
        IRepository<Category> categoryRepository,
        IRepository<UnitOfMeasure> unitOfMeasureRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfMeasureRepository = unitOfMeasureRepository;
    }

    public async Task ValidateSkuAsync<T>(
        string sku,
        ValidationContext<T> context,
        CancellationToken ct,
        string propertyName = "SKU")
    {
        if (await _productRepository.GetBySkuAsync(sku, ct) != null)
        {
            context.AddFailure(propertyName, "SKU must be unique");
        }
    }

    public async Task ValidateCategoryAsync<T>(
        int categoryId,
        ValidationContext<T> context,
        CancellationToken ct,
        string propertyName = "CategoryId")
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, ct);
        if (category == null || category.IsDeleted)
        {
            context.AddFailure(propertyName, "Category not found or was deleted");
        }
    }

    public async Task ValidateUnitOfMeasureAsync<T>(
        int unitOfMeasureId,
        ValidationContext<T> context,
        CancellationToken ct,
        string propertyName = "UnitOfMeasureId")
    {
        var unitOfMeasure = await _unitOfMeasureRepository.GetByIdAsync(unitOfMeasureId, ct);
        if ( unitOfMeasure == null || unitOfMeasure.IsDeleted)
        {
            context.AddFailure(propertyName, "Unit of measure not found or was deleted");
        }
    }
}