using FluentValidation;
using Inventory.API.Validators.ValueObjects;
using Inventory.Shared.DTOs.Warehouses;

namespace Inventory.API.Validators.Warehouses;

public class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseRequestDto>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(warehouse => warehouse.WarehouseCode)
            .Cascade(CascadeMode.Stop) // Updated from CascadeMode.Stop
            .NotEmpty().WithMessage("Warehouse code cannot be empty")
            .Length(3, 15).WithMessage("Warehouse code must be 3-15 characters")
            .Must(code => ValidationHelper.MatchPattern(code, @"^[A-Z0-9\-_]+$"))
            .WithMessage("Warehouse code can only contain uppercase letters, numbers, hyphens, and underscores")
            .Must(code => !ValidationHelper.ContainsReservedWords(code))
            .WithMessage("Warehouse code contains prohibited words")
            .Must(code => !ValidationHelper.ContainsHtmlTags(code))
            .WithMessage("Warehouse code cannot contain HTML tags")
            .Must(code => !ValidationHelper.ContainsSqlInjectionPatterns(code))
            .WithMessage("Warehouse code cannot contain unsafe content")
            .Must(code => !ValidationHelper.ContainsEmoji(code))
            .WithMessage("Warehouse code cannot contain emojis");
        
        RuleFor(warehouse => warehouse.WarehouseName)
            .Cascade(CascadeMode.Stop) // Updated from CascadeMode.Stop
            .NotEmpty().WithMessage("Warehouse name is required")
            .Length(2, 100).WithMessage("Warehouse name must be 2-100 characters")
            .Must(name => ValidationHelper.BeAValidName(name, 2, 100))
            .WithMessage("Name contains invalid characters")
            .Must(name => !ValidationHelper.ContainsReservedWords(name))
            .WithMessage("Name contains prohibited words")
            .Must(name => !ValidationHelper.ContainsHtmlTags(name))
            .WithMessage("Name cannot contain HTML tags")
            .Must(name => !ValidationHelper.ContainsSqlInjectionPatterns(name))
            .WithMessage("Name cannot contain unsafe content")
            .Must(name => !ValidationHelper.ContainsEmoji(name))
            .WithMessage("Name cannot contain emojis");
        
        RuleFor(warehouse => warehouse.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0")
            .LessThanOrEqualTo(1000000).WithMessage("Capacity cannot exceed 1,000,000");
    }
}