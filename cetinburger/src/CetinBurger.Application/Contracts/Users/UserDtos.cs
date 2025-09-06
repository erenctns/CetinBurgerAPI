namespace CetinBurger.Application.Contracts.Users;

/// <summary>
/// Kullanıcı listesi için DTO (pagination ile)
/// </summary>
public class UserListDto
{
    public List<UserDto> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}

/// <summary>
/// Tekil kullanıcı bilgileri için DTO
/// </summary>
public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Kullanıcı rolü güncelleme için DTO
/// </summary>
public class UpdateUserRoleRequestDto
{
    public string Role { get; set; } = string.Empty;
}
