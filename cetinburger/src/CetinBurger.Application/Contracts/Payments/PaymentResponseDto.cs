namespace CetinBurger.Application.Contracts.Payments;

/// <summary>
/// Stripe ödeme işlemi sonucu için response DTO'su
/// </summary>
public class PaymentResponseDto
{

    /// Ödeme başarılı mı?
    
    public bool IsSuccess { get; set; }
    
 
    /// Stripe PaymentIntent ID'si
   
    public string PaymentIntentId { get; set; } = string.Empty;
    
   
    /// Ödeme durumu (succeeded, processing, requires_payment_method, etc.)
    
    public string Status { get; set; } = string.Empty;
    
    
    /// Ödeme tutarı (kuruş cinsinden)
    
    public long Amount { get; set; }
    
    
    /// Para birimi
    
    public string Currency { get; set; } = string.Empty;
    
    
    /// Hata mesajı (başarısız ise)
    
    public string ErrorMessage { get; set; } = string.Empty;
    
    
    /// Ödeme tarihi
   
    public DateTime CreatedAt { get; set; }
    
   
    /// Müşteri email'i
   
    public string CustomerEmail { get; set; } = string.Empty;
}
