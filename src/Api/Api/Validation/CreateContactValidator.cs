using Api.Dtos;
using FluentValidation;

namespace Api.Validation;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("First name is required and must be less than 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Last name is required and must be less than 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress()
            .WithMessage("Valid email address is required.");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .Matches(@"^\d{3}-\d{3}-\d{4}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone number must be in format 555-555-5555.");

        RuleFor(x => x.Company)
            .MaximumLength(200)
            .WithMessage("Company name must be less than 200 characters.");

        RuleFor(x => x.AddressLine1)
            .MaximumLength(255)
            .WithMessage("Address line 1 must be less than 255 characters.");

        RuleFor(x => x.AddressLine2)
            .MaximumLength(255)
            .WithMessage("Address line 2 must be less than 255 characters.");

        RuleFor(x => x.City)
            .MaximumLength(100)
            .WithMessage("City must be less than 100 characters.");

        RuleFor(x => x.State)
            .MaximumLength(100)
            .WithMessage("State must be less than 100 characters.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .WithMessage("Postal code must be less than 20 characters.");

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .WithMessage("Country must be less than 100 characters.");
    }
}

