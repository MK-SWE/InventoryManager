using FluentValidation;
using Inventory.Application.Products.Commands;
using Inventory.Domain.Interfaces;

namespace Inventory.Application.Products.Validators;

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
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null || product.IsDeleted)
        {
            context.AddFailure("ProductId", "Product not found or was deleted");
            return;
        }

        var tasks = new List<Task>();

        if (!string.IsNullOrEmpty(command.UpdateProductCommandDto.SKU) 
            && !command.UpdateProductCommandDto.SKU.Equals(product.SKU, StringComparison.OrdinalIgnoreCase))
        {
            tasks.Add(_validationHelper.ValidateSkuAsync(
                command.UpdateProductCommandDto.SKU,
                context,
                cancellationToken
            ));
        }

        if (command.UpdateProductCommandDto.CategoryId.HasValue)
        {
            tasks.Add(_validationHelper.ValidateCategoryAsync(
                command.UpdateProductCommandDto.CategoryId.Value,
                context,
                cancellationToken
            ));
        }

        if (command.UpdateProductCommandDto.UnitOfMeasureId.HasValue)
        {
            tasks.Add(_validationHelper.ValidateUnitOfMeasureAsync(
                command.UpdateProductCommandDto.UnitOfMeasureId.Value,
                context,
                cancellationToken
            ));
        }

        await Task.WhenAll(tasks);
    }
}
