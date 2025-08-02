using FluentValidation;
using Inventory.Application.Products.Commands;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.Products.Validators;

// UpdateProductCommandValidator.cs
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly ProductValidationHelper _validationHelper;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandValidator(
        IProductRepository productRepository,
        ProductValidationHelper validationHelper) // Remove <T>
    {
        _validationHelper = validationHelper;
        _productRepository = productRepository;

        RuleFor(command => command)
            .CustomAsync(ParallelValidation);
    }

    private async Task ParallelValidation(
        UpdateProductCommand command,
        ValidationContext<UpdateProductCommand> context,
        CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, ct);
        if (product == null || product.IsDeleted)
        {
            context.AddFailure("ProductId", "Product not found or was deleted");
            return;
        }

        var tasks = new List<Task>();

        if (!string.IsNullOrEmpty(command.UpdateProductDto.SKU) 
            && !command.UpdateProductDto.SKU.Equals(product.SKU, StringComparison.OrdinalIgnoreCase))
        {
            tasks.Add(_validationHelper.ValidateSkuAsync(
                command.UpdateProductDto.SKU,
                context,
                ct
            ));
        }

        if (command.UpdateProductDto.CategoryId.HasValue)
        {
            tasks.Add(_validationHelper.ValidateCategoryAsync(
                command.UpdateProductDto.CategoryId.Value,
                context,
                ct
            ));
        }

        if (command.UpdateProductDto.UnitOfMeasureId.HasValue)
        {
            tasks.Add(_validationHelper.ValidateUnitOfMeasureAsync(
                command.UpdateProductDto.UnitOfMeasureId.Value,
                context,
                ct
            ));
        }

        await Task.WhenAll(tasks);
    }
}
