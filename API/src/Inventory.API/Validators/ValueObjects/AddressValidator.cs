using FluentValidation;
using Inventory.Domain.ValueObjects;

namespace Inventory.API.Validators.ValueObjects;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.Line1)
            .NotEmpty().WithMessage("Address line 1 is required")
            .MaximumLength(100).WithMessage("Address line 1 cannot exceed 100 characters")
            .Must(line => ValidationHelper.MatchPattern(line, @"^[\p{L}0-9\s\-\#\.\,]+$"))
            .WithMessage("Address line 1 contains invalid characters");

        RuleFor(address => address.Line2)
            .MaximumLength(100).WithMessage("Address line 2 cannot exceed 100 characters")
            .Must(line => ValidationHelper.MatchPattern(line, @"^[\p{L}0-9\s\-\#\.\,]+$"))
            .When(a => !string.IsNullOrEmpty(a.Line2))
            .WithMessage("Address line 2 contains invalid characters");

        RuleFor(address => address.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(50).WithMessage("City cannot exceed 50 characters")
            .Must(city => ValidationHelper.MatchPattern(city, @"^[\p{L}\s\-\'\.]+$"))
            .WithMessage("City contains invalid characters");

        RuleFor(address => address.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(50).WithMessage("State cannot exceed 50 characters")
            .Must(state => ValidationHelper.MatchPattern(state, @"^[\p{L}\s\-]+$"))
            .WithMessage("State contains invalid characters");

        RuleFor(address => address.PostalCode)
            .NotEmpty().WithMessage("Postal code is required")
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters")
            .Must(postalCode => ValidationHelper.MatchPattern(postalCode, @"^[A-Z0-9\-\s]{3,10}$"))
            .WithMessage("Postal code format is invalid");
        
        RuleFor(address => address.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(50).WithMessage("Country cannot exceed 50 characters")
            .Must(country => ValidationHelper.MatchPattern(country, @"^[\p{L}\s\-\'\.]+$"))
            .WithMessage("Country contains invalid characters");
    }
}