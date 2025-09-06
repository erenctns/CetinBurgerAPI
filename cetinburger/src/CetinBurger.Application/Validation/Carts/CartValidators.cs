using CetinBurger.Application.Contracts.Carts;
using FluentValidation;

namespace CetinBurger.Application.Validation.Carts;

public class AddCartItemRequestDtoValidator : AbstractValidator<AddCartItemRequestDto>
{
    public AddCartItemRequestDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be a positive integer.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}

public class UpdateCartItemRequestDtoValidator : AbstractValidator<UpdateCartItemRequestDto>
{
    public UpdateCartItemRequestDtoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}


