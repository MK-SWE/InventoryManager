using FluentValidation;
using Inventory.Shared.DTOs.Products;
using System.Text.RegularExpressions;

namespace Inventory.API.Validators.Products;

public class CreateProductRequestDtoValidator : AbstractValidator<CreateProductRequestDto>
{
    public CreateProductRequestDtoValidator()
    {
        RuleFor(product => product.SKU)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("SKU is required")
            .Length(3, 50).WithMessage("SKU must be 3-50 characters")
            .Must(sku => ValidationHelper.MatchPattern(sku ,@"^[a-zA-Z0-9\-_]+$")).WithMessage("SKU can only contain letters, numbers, dashes, and underscores")
            .Must(sku => !sku!.All(char.IsDigit)).WithMessage("SKU cannot be only numbers")
            .Must(sku => !Regex.IsMatch(sku!, @"(.)\1{4,}")).WithMessage("SKU cannot have 5+ repeating characters")
            .Must(sku => !ValidationHelper.ContainsHtmlTags(sku)).WithMessage("SKU cannot contains HTML tags")
            .Must(sku => !ValidationHelper.ContainsSqlInjectionPatterns(sku)).WithMessage("SKU cannot contains unsafe content")
            .Must(sku => !ValidationHelper.ContainsEmoji(sku)).WithMessage("SKU cannot contain emojis");
        
        RuleFor(product => product.ProductName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("ProductName is required")
            .Length(2, 100).WithMessage("ProductName must be 2-100 characters")
            .Must(name => ValidationHelper.BeAValidName(name)).WithMessage("Name contains invalid characters")
            .Must(name => !ValidationHelper.ContainsReservedWords(name!)).WithMessage("Name contains prohibited words")
            .Must(name => !ValidationHelper.ContainsHtmlTags(name)).WithMessage("Name cannot contains HTML tags")
            .Must(name => !ValidationHelper.ContainsSqlInjectionPatterns(name)).WithMessage("Name cannot contains unsafe content")
            .Must(name => !ValidationHelper.ContainsEmoji(name)).WithMessage("Name cannot contain emojis");
        
        RuleFor(product => product.ProductDescription)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 2000).WithMessage("Description must be 10-2000 characters")
            .Must(desc => !ValidationHelper.ContainsHtmlTags(desc)).WithMessage("Description cannot contain HTML tags")
            .Must(desc => !ValidationHelper.ContainsSqlInjectionPatterns(desc)).WithMessage("Description contains unsafe content")
            .Must(desc => !ValidationHelper.ContainsEmoji(desc)).WithMessage("Description cannot contain emojis");

        RuleFor(product => product.UnitCost)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Cost is required")
            .Must(price => ValidationHelper.BeAValidPrice(price)).WithMessage("Cost must be 0-10,000,000 with 2 decimal places");

        RuleFor(product => product.UnitPrice)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Price is required")
            .Must(price => ValidationHelper.BeAValidPrice(price)).WithMessage("Price must be 0-10,000,000 with 2 decimal places")
            .GreaterThan(product => product.UnitCost).WithMessage("Unit price must be greater than unit cost");
        
        RuleFor(product => product.ReorderLevel)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Reorder Level is required")
            .Must(quantity => ValidationHelper.BeAValidQuantity(quantity))
            .When(product => product.ReorderLevel.HasValue).WithMessage("Reorder level must be 0-100,000");
        
        RuleFor(product => product.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Category is required")
            .GreaterThan(0).WithMessage("Category ID must be positive")
            .Must(catId => catId < 10_000_000).WithMessage("Invalid category ID");
        
        RuleFor(product => product.Weight)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Weight is required")
            .GreaterThan(0).WithMessage("Weight must be > 0")
            .LessThan(1000).WithMessage("Weight exceeds maximum allowed");

        RuleFor(product => product.Volume)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Volume is required")
            .GreaterThan(0).WithMessage("Volume must be > 0")
            .LessThan(100).WithMessage("Volume exceeds maximum allowed");
    }
}