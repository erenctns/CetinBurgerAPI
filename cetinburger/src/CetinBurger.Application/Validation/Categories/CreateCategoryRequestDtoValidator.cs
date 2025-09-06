using CetinBurger.Application.Contracts.Categories;
using FluentValidation;

namespace CetinBurger.Application.Validation.Categories;

public class CreateCategoryRequestDtoValidator : AbstractValidator<CreateCategoryRequestDto>
{
    public CreateCategoryRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name length must be <= 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description length must be <= 500 characters.");
    }
}


