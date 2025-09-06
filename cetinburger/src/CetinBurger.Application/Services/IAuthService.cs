using CetinBurger.Application.Contracts.Auth;

namespace CetinBurger.Application.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
