using CetinBurger.Application.Contracts.Auth;
using FluentValidation;

namespace CetinBurger.Application.Validation.Auth;

// Login isteği için basit doğrulama kuralları
public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}


