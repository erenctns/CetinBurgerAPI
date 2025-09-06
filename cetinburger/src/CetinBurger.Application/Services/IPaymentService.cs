using CetinBurger.Application.Contracts.Payments;

namespace CetinBurger.Application.Services;

/// Ödeme işlemleri için service interface'i
public interface IPaymentService
{
    /// Stripe ile ödeme işlemini gerçekleştir
    /// <param name="request">Ödeme bilgileri</param>
    /// <returns>Ödeme sonucu</returns>
    Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request);
    
    /// Ödeme durumunu kontrol et
    /// <param name="paymentIntentId">Stripe PaymentIntent ID'si</param>
    /// <returns>Ödeme durumu</returns>
    Task<PaymentResponseDto> GetPaymentStatusAsync(string paymentIntentId);

    /// Ödeme işlemini iptal et
    /// <param name="paymentIntentId">Stripe PaymentIntent ID'si</param>
    /// <returns>İptal sonucu</returns>
    Task<PaymentResponseDto> CancelPaymentAsync(string paymentIntentId);
}

