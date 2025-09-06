using CetinBurger.Application.Contracts.Auth;
using CetinBurger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetinBurger.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	/// Yeni kullanıcı kaydı. Doğrulamalar FluentValidation ile yapılır.
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
	{
		try
		{
			await _authService.RegisterAsync(request);
			return Ok();
		}
		catch (InvalidOperationException ex)
		{
			if (ex.Message.Contains("Email is already registered"))
			{
				return Conflict(ex.Message);
			}
			if (ex.Message.Contains("User creation failed"))
			{
				return BadRequest(ex.Message);
			}
			return BadRequest(ex.Message);
		}
	}

	/// Kullanıcı girişi yapar ve JWT döner.
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
	{
		try
		{
			var token = await _authService.LoginAsync(request);
			return Ok(token);
		}
		catch (InvalidOperationException ex)
		{
			if (ex.Message.Contains("Invalid credentials"))
			{
				return Unauthorized(ex.Message);
			}
			return BadRequest(ex.Message);
		}
	}
}


