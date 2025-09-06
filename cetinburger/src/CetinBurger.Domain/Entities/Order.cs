using System.Text.Json.Serialization;

namespace CetinBurger.Domain.Entities;


//kullanıcının verdiği siparişin ana kaydı. siparişin durumu toplam tutarı gibi bilgiler burda tutulur.
public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Status { get; set; } = "PendingPayment"; // Baslangic durumu
    public decimal TotalAmount { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Note { get; set; }
    public bool MailSentStatus { get; set; } = false; // Mail gönderim durumu
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}


