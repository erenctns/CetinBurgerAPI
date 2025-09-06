namespace CetinBurger.Application.Contracts.Products;

// Bu DTO, API'nin Product güncelleme (PUT /api/products/{id}) isteği için kullanılır.
public class UpdateProductRequestDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
}


