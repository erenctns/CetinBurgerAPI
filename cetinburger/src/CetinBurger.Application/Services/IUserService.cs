using CetinBurger.Application.Contracts.Users;

namespace CetinBurger.Application.Services;

/// <summary>
/// Kullanıcı yönetimi için service interface'i
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Tüm kullanıcıları sayfalı olarak getir
    /// </summary>
    Task<UserListDto> GetAllAsync(int page = 1, int pageSize = 20);

    /// <summary>
    /// Belirli bir kullanıcıyı ID ile getir
    /// </summary>
    Task<UserDto?> GetByIdAsync(string id);

    /// <summary>
    /// Kullanıcının rolünü güncelle
    /// </summary>
    Task<UserDto> UpdateRoleAsync(string id, string role);

    /// <summary>
    /// Kullanıcıyı sil
    /// </summary>
    Task DeleteAsync(string id);
}
