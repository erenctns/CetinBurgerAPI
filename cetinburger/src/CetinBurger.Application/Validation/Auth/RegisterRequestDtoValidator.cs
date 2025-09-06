using CetinBurger.Application.Contracts.Auth;
using FluentValidation;

namespace CetinBurger.Application.Validation.Auth;

// Register isteği için doğrulama kuralları
public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name length must be <= 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name length must be <= 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200).WithMessage("Address length must be <= 200 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Must(IsValidTrPhone).WithMessage("PhoneNumber must be a valid Turkish phone number (e.g., 05555555555 or 0555 555 55 55).");
    }

    /// <summary>
    /// Türk telefon numarası formatını kontrol eder (sadece 0 ile başlayan)
    /// </summary>
    private bool IsValidTrPhone(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        
        // Sadece rakamları al (boşlukları ve diğer karakterleri kaldır)
        var digits = new string(input.Where(char.IsDigit).ToArray());
        
        // Türk telefon numarası formatı: 0 ile başlayan 11 hane
        // Örnekler: 05555555555, 0555 555 55 55, 0555-555-55-55
        
        return digits.Length == 11 && digits.StartsWith("0");
    }
}


