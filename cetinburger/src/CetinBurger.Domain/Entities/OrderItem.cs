using System.Text.Json.Serialization;

namespace CetinBurger.Domain.Entities;


// Order ' da toplam sipariş kayıt edilir fakat OrderItem'da her bir ürün ayrı ayrı kaydedilir, mesela hem kola hem burger için 2 kayıt var burda ama orderda tek bir kayıt var.
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? ProductId { get; set; } // Opsiyonel FK (ürün silinse de geçmiş korunur)
    public string ProductName { get; set; } = string.Empty; // Snapshot
    public decimal UnitPrice { get; set; } // Snapshot
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public Order? Order { get; set; }
}


