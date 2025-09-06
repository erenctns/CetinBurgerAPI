using CetinBurger.Application.Contracts.Orders;
using FluentValidation;

namespace CetinBurger.Application.Validation.Orders;

public class UpdateMailSentStatusRequestDtoValidator : AbstractValidator<UpdateMailSentStatusRequestDto>
{
    public UpdateMailSentStatusRequestDtoValidator()
    {
        // MailSentStatus boolean değeri için özel bir validation gerekmez
        // Çünkü boolean zaten true/false değerlerini kabul eder
        // Ancak açıklayıcı mesaj için kural ekleyebiliriz
        
        RuleFor(x => x.MailSentStatus)
            .NotNull()
            .WithMessage("Mail gönderim durumu belirtilmelidir.");
    }
}
