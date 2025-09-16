using FluentValidation;
using Inventory.Shared.DTOs.Products;

namespace Inventory.API.Validators.Products;

public class UpdateProductRequestDtoValidator: AbstractValidator<UpdateProductRequestDto>
{
    public UpdateProductRequestDtoValidator()
    {
        When(product => product.ProductName != null, () =>
        {
            RuleFor(product => product.ProductName)
                .Cascade(CascadeMode.Stop)
                .Length(2, 100).WithMessage("ProductName must be 2-100 characters")
                .Must(name => ValidationHelper.BeAValidName(name, minLength: 2, maxLength: 100)).WithMessage("Name contains invalid characters")
                .Must(name => !ValidationHelper.ContainsReservedWords(name)).WithMessage("Name contains prohibited words")
                .Must(name => !ValidationHelper.ContainsHtmlTags(name)).WithMessage("Name cannot contains HTML tags")
                .Must(name => !ValidationHelper.ContainsSqlInjectionPatterns(name)).WithMessage("Name cannot contains unsafe content")
                .Must(name => !ValidationHelper.ContainsEmoji(name)).WithMessage("Name cannot contain emojis");
        });
        
        When(product => product.ProductDescription != null, () =>
        {
            RuleFor(product => product.ProductDescription)
                .Cascade(CascadeMode.Stop)
                .Length(10, 2000).WithMessage("Description must be 10-2000 characters")
                .Must(desc => !ValidationHelper.ContainsHtmlTags(desc)).WithMessage("Description cannot contain HTML tags")
                .Must(desc => !ValidationHelper.ContainsSqlInjectionPatterns(desc)).WithMessage("Description contains unsafe content")
                .Must(desc => !ValidationHelper.ContainsEmoji(desc!)).WithMessage("Description cannot contain emojis");
        });
        
        When(product => product.UnitPrice != null, () =>
        {
            RuleFor(product => product.UnitPrice)
                .Cascade(CascadeMode.Stop)
                .Must(price => ValidationHelper.BeAValidPrice(price)).WithMessage("Price must be 0-10,000,000 with 2 decimal places");
        });
        
        When(product => product.ReorderLevel != null, () =>
        {
            RuleFor(product => product.ReorderLevel)
                .Must(quantity => ValidationHelper.BeAValidQuantity(quantity))
                .When(product => product.ReorderLevel.HasValue).WithMessage("Reorder level must be 0-100,000");
        });
    }
}