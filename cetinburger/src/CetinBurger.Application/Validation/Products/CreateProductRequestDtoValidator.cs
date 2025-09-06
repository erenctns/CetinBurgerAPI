using CetinBurger.Application.Contracts.Products;
using FluentValidation;

namespace CetinBurger.Application.Validation.Products;

public class CreateProductRequestDtoValidator : AbstractValidator<CreateProductRequestDto>
{
    public CreateProductRequestDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be a positive integer.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150).WithMessage("Name length must be <= 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description length must be <= 1000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl length must be <= 500 characters.");
    }
}


