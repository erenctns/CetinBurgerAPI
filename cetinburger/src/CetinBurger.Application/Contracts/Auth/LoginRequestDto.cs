namespace CetinBurger.Application.Contracts.Auth;

// Bu DTO, API'de kullanıcı giriş (login) isteği için kullanılır.
public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}


