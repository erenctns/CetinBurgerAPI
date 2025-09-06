namespace CetinBurger.Application.Contracts.Products;

// Bu DTO, API'nin Product oluşturma (POST /api/products) isteği için kullanılır.
// Presentation katmanı yalnızca gerekli alanları alır; Entity doğrudan dışa açılmaz.
public class CreateProductRequestDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
}


