namespace CetinBurger.Application.Contracts.Products;

// Bu DTO, Product varlıklarının dışarıya (API response) aktarımı için kullanılır.
public class ProductDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string? ImageUrl { get; set; }
}


