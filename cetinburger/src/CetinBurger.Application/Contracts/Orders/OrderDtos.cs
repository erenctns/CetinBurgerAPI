namespace CetinBurger.Application.Contracts.Orders;

// Request: Sepetten sipariş oluşturma (ödeme bilgileri olmadan)

//ödeme ekranında kullanıcıdan aldığımız veriler , controller'ı direkt domaindaki entitye marus bırakmamak için gerekenleri dto ile alırız.
public class CheckoutRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Note { get; set; }
}

// Response: Sipariş özet
//Sipariş listesini dışarıya dönücez , siparişi
public class OrderDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool MailSentStatus { get; set; } // Mail gönderim durumu
    public List<OrderItemDto> Items { get; set; } = new();
}

//siparişteki her bir ürünün bilgileri
public class OrderItemDto
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

// Admin için: Tüm siparişleri listeleme (kullanıcı bilgileri dahil)
public class AdminOrderListDto
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty; // FirstName + LastName
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; } // Toplam ürün sayısı
    public bool MailSentStatus { get; set; } // Mail gönderim durumu
}

// Admin için: Sipariş durumu güncelleme
public class UpdateOrderStatusRequestDto
{
    public string Status { get; set; } = string.Empty;
}

// Admin için: Mail gönderim durumu güncelleme
public class UpdateMailSentStatusRequestDto
{
    public bool MailSentStatus { get; set; }
}

// Admin için: Sipariş silme
public class DeleteOrderRequestDto
{
    public int Id { get; set; }
}


