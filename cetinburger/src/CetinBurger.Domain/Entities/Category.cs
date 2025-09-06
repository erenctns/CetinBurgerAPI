using System.Text.Json.Serialization;

namespace CetinBurger.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    //product navigation propertysi
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>(); //bir kategoriye birden fazla ürün eklenebilir
}



