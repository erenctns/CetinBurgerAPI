using FluentValidation;
using CetinBurger.Application.Contracts.Orders;

namespace CetinBurger.Application.Validation.Orders;

public class DeleteOrderRequestDtoValidator : AbstractValidator<DeleteOrderRequestDto>
{
    public DeleteOrderRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Order ID 0'dan büyük olmalıdır.");
    }
}
