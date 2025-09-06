using CetinBurger.Application.Contracts.Orders;
using FluentValidation;

namespace CetinBurger.Application.Validation.Orders;

public class CheckoutRequestDtoValidator : AbstractValidator<CheckoutRequestDto>
{
    public CheckoutRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name length must be <= 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name length must be <= 50 characters.");

        RuleFor(x => x.DeliveryAddress)
            .NotEmpty().WithMessage("Delivery address is required.")
            .MaximumLength(200).WithMessage("Delivery address length must be <= 200 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Must(IsValidTrPhone).WithMessage("Phone number must be a valid Turkish phone number (e.g., 05555555555 or 0555 555 55 55).");

        RuleFor(x => x.Note)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Note))
            .WithMessage("Note length must be <= 500 characters.");
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


