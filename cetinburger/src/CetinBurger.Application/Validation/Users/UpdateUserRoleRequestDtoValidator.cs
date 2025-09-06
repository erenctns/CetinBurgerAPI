using CetinBurger.Application.Contracts.Users;
using FluentValidation;

namespace CetinBurger.Application.Validation.Users;

/// <summary>
/// Kullanıcı rolü güncelleme için validation kuralları
/// </summary>
public class UpdateUserRoleRequestDtoValidator : AbstractValidator<UpdateUserRoleRequestDto>
{
    public UpdateUserRoleRequestDtoValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(IsValidRole).WithMessage("Role must be either 'Admin' or 'Customer'.");
    }

    /// <summary>
    /// Geçerli rol kontrolü
    /// </summary>
    private bool IsValidRole(string role)
    {
        return role == "Admin" || role == "Customer";
    }
}
