using System.Text.Json.Serialization;

namespace CetinBurger.Domain.Entities;

// her kullanıcı için tek bir sepet var.
public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}


