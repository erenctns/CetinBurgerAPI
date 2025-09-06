using CetinBurger.Application.Contracts.Payments;
using FluentValidation;

namespace CetinBurger.Application.Validation.Payments;

public class PaymentRequestDtoValidator : AbstractValidator<PaymentRequestDto>
{
    public PaymentRequestDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be exactly 3 characters (e.g., TRY, USD, EUR).");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description length must be <= 200 characters.");

        RuleFor(x => x.CustomerEmail)
            .NotEmpty().WithMessage("Customer email is required.")
            .EmailAddress().WithMessage("Customer email must be a valid email address.")
            .MaximumLength(100).WithMessage("Customer email length must be <= 100 characters.");

        RuleFor(x => x.PaymentMethodId)
            .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.PaymentMethodId))
            .WithMessage("PaymentMethodId length must be <= 100 characters.");

        RuleFor(x => x.OrderId)
            .GreaterThan(0).When(x => x.OrderId.HasValue)
            .WithMessage("OrderId must be greater than 0 when provided.");
    }
}
