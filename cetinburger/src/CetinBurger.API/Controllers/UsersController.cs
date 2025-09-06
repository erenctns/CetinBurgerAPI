using CetinBurger.Application.Contracts.Users;
using CetinBurger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetinBurger.API.Controllers;

/// <summary>
/// Kullanıcı yönetimi için controller (Sadece Admin erişimi)
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Tüm kullanıcıları sayfalı olarak getir
    /// </summary>
    /// <param name="page">Sayfa numarası (varsayılan: 1)</param>
    /// <param name="pageSize">Sayfa başına kullanıcı sayısı (varsayılan: 20, maksimum: 100)</param>
    [HttpGet]
    public async Task<ActionResult<UserListDto>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var users = await _userService.GetAllAsync(page, pageSize);
            return Ok(users);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Belirli bir kullanıcıyı ID ile getir
    /// </summary>
    /// <param name="id">Kullanıcı ID'si</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById([FromRoute] string id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User {id} not found.");
            }

            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Kullanıcının rolünü güncelle
    /// </summary>
    /// <param name="id">Kullanıcı ID'si</param>
    /// <param name="request">Rol güncelleme isteği</param>
    [HttpPut("{id}/role")]
    public async Task<ActionResult<UserDto>> UpdateRole([FromRoute] string id, [FromBody] UpdateUserRoleRequestDto request)
    {
        try
        {
            var user = await _userService.UpdateRoleAsync(id, request.Role);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("not found"))
            {
                return NotFound(ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Kullanıcıyı sil
    /// </summary>
    /// <param name="id">Kullanıcı ID'si</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("not found"))
            {
                return NotFound(ex.Message);
            }
            if (ex.Message.Contains("Cannot delete admin"))
            {
                return Forbid(ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }
}
