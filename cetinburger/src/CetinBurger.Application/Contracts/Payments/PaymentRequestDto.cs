namespace CetinBurger.Application.Contracts.Payments;


/// Stripe ödeme işlemi için request DTO'su

public class PaymentRequestDto
{
    
    /// Ödenecek tutar (kuruş cinsinden - 100 = 1 TL)
    
    public int Amount { get; set; }
    /// Para birimi (TRY, USD, EUR)
    public string Currency { get; set; } = "try";
    /// Stripe'dan gelen payment method ID (opsiyonel - boş bırakılırsa test kartı kullanılır)
    public string? PaymentMethodId { get; set; }
    /// Ödeme açıklaması
    public string Description { get; set; } = string.Empty;
    /// Sipariş ID'si (opsiyonel)
    public int? OrderId { get; set; }
    /// Müşteri email'i
    public string CustomerEmail { get; set; } = string.Empty;
}

