using CetinBurger.Application.Contracts.Users;
using CetinBurger.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CetinBurger.Application.Services;

/// <summary>
/// Kullanıcı yönetimi için service implementation'ı
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Tüm kullanıcıları sayfalı olarak getir
    /// </summary>
    public async Task<UserListDto> GetAllAsync(int page = 1, int pageSize = 20)
    {
        // Sayfa parametrelerini doğrula
        page = Math.Max(1, page);
        pageSize = Math.Max(1, Math.Min(100, pageSize)); // Maksimum 100 kullanıcı

        // Toplam kullanıcı sayısını al
        var totalCount = await _userManager.Users.CountAsync();

        // Sayfalama hesaplamaları
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var skip = (page - 1) * pageSize;

        // Kullanıcıları sayfalı olarak getir
        var users = await _userManager.Users
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        // Her kullanıcının rollerini al ve AutoMapper ile DTO'ya dönüştür
        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();
            userDtos.Add(userDto);
        }

        return new UserListDto
        {
            Data = userDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasNext = page < totalPages,
            HasPrevious = page > 1
        };
    }

    /// <summary>
    /// Belirli bir kullanıcıyı ID ile getir
    /// </summary>
    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();
        return userDto;
    }

    /// <summary>
    /// Kullanıcının rolünü güncelle
    /// </summary>
    public async Task<UserDto> UpdateRoleAsync(string id, string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException($"User {id} not found.");
        }

        // Rolün geçerli olup olmadığını kontrol et
        if (!await _roleManager.RoleExistsAsync(role))
        {
            throw new InvalidOperationException($"Role '{role}' does not exist.");
        }

        // Mevcut rolleri al
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Tüm rolleri kaldır
        if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        // Yeni rolü ekle
        await _userManager.AddToRoleAsync(user, role);

        // Güncellenmiş kullanıcı bilgilerini döndür
        return await GetByIdAsync(id) ?? throw new InvalidOperationException("Failed to retrieve updated user.");
    }

    /// <summary>
    /// Kullanıcıyı sil
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException($"User {id} not found.");
        }

        // Admin kendini silemesin
        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (isAdmin)
        {
            throw new InvalidOperationException("Cannot delete admin users.");
        }

        // Kullanıcıyı sil
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to delete user: {errors}");
        }
    }
}
