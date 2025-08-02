using FluentValidation;
using Inventory.Application.Products.Commands;

namespace Inventory.Application.Products.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly ProductValidationHelper _validationHelper;

    public CreateProductCommandValidator(ProductValidationHelper validationHelper) // Remove <T>
    {
        _validationHelper = validationHelper;

        RuleFor(command => command)
            .CustomAsync(ParallelValidation);
    }

    private async Task ParallelValidation(
        CreateProductCommand command, 
        ValidationContext<CreateProductCommand> context, 
        CancellationToken ct)
    {
        await _validationHelper.ValidateSkuAsync(command.CreateProductDto.SKU, context, ct);
        await _validationHelper.ValidateCategoryAsync(command.CreateProductDto.CategoryId, context, ct);
        await _validationHelper.ValidateUnitOfMeasureAsync(command.CreateProductDto.UnitOfMeasureId, context, ct);
    }
}
