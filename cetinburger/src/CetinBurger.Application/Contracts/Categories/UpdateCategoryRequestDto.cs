namespace CetinBurger.Application.Contracts.Categories;

// Bu DTO, API'nin Category güncelleme (PUT /api/categories/{id}) isteği için kullanılır.
// Presentation (API) sadece gerekli alanları alır; Entity doğrudan dışarı açılmaz.
public class UpdateCategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}


