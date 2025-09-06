namespace CetinBurger.Application.Contracts.Categories;

// Bu DTO, API'nin Category oluşturma (POST /api/categories) isteği için kullanılır.
// Controller'ların içinde model tanımı tutmamak (Clean Architecture) için
// Presentation katmanı bu sözleşmeyi Application katmanından tüketir.
public class CreateCategoryRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}


