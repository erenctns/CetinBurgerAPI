namespace CetinBurger.Application.Contracts.Auth;

// Bu DTO, API'de login yanıtı için kullanılır; JWT ve bitiş zamanını taşır.
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}


