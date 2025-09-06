using CetinBurger.Application.Contracts.Payments;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace CetinBurger.Application.Services;

/// <summary>
/// Stripe ödeme sistemi implementation'ı
/// </summary>
public class StripePaymentService : IPaymentService
{
    private readonly string _secretKey;
    private readonly IConfiguration _configuration;

    public StripePaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        // appsettings.json'dan Stripe key al
        _secretKey = _configuration["Stripe:SecretKey"] 
            ?? throw new InvalidOperationException("Stripe SecretKey not configured in appsettings.json");
        
        // Stripe API key'ini ayarla
        StripeConfiguration.ApiKey = _secretKey;
    }
    /// Test kartı için PaymentMethod oluşturur, Stripe'ın test token'ını kullanır
    private async Task<string> CreateTestPaymentMethodAsync()
    {
        // Stripe'ın test token'ını kullan (güvenli yöntem)
        var options = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Token = "tok_visa", // Stripe'ın test token'ı
            },
        };

        var service = new PaymentMethodService();
        var paymentMethod = await service.CreateAsync(options);
        
        return paymentMethod.Id;
    }

    public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
    {
        try
        {
            // Eğer PaymentMethodId verilmemişse test kartı oluştur
            string paymentMethodId = request.PaymentMethodId ?? string.Empty;
            if (string.IsNullOrEmpty(paymentMethodId))
            {
                paymentMethodId = await CreateTestPaymentMethodAsync();
            }

            // PaymentIntent oluştur
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency.ToLower(),
                PaymentMethod = paymentMethodId,
                Confirm = true,
                Description = request.Description,
                ReceiptEmail = request.CustomerEmail,
                //Şimdi normalde stripeda ödemeyi yaptıktan sonra seni başka sayfaya atar başarıyla gerçekleşti gibi birşey gösterir ama bizde front-end olmadığı için
                //yönlendirmemesini sağladık, normalde akış şöyle: Ödeme → Banka Sayfası → Doğrulama → Geri Dön → Başarılı
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never" // Yönlendirme yok, direkt sonuç
                },
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", request.OrderId?.ToString() ?? "N/A" },
                    { "CustomerEmail", request.CustomerEmail }
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return new PaymentResponseDto
            {
                IsSuccess = paymentIntent.Status == "succeeded",
                PaymentIntentId = paymentIntent.Id,
                Status = paymentIntent.Status,
                Amount = paymentIntent.Amount,
                Currency = paymentIntent.Currency,
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = request.CustomerEmail,
                ErrorMessage = paymentIntent.Status == "succeeded" ? string.Empty : "Payment processing failed"
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResponseDto
            {
                IsSuccess = false,
                PaymentIntentId = string.Empty,
                Status = "failed",
                Amount = request.Amount,
                Currency = request.Currency,
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = request.CustomerEmail,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            return new PaymentResponseDto
            {
                IsSuccess = false,
                PaymentIntentId = string.Empty,
                Status = "error",
                Amount = request.Amount,
                Currency = request.Currency,
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = request.CustomerEmail,
                ErrorMessage = $"Unexpected error: {ex.Message}"
            };
        }
    }

    public async Task<PaymentResponseDto> GetPaymentStatusAsync(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);

            return new PaymentResponseDto
            {
                IsSuccess = paymentIntent.Status == "succeeded",
                PaymentIntentId = paymentIntent.Id,
                Status = paymentIntent.Status,
                Amount = paymentIntent.Amount,
                Currency = paymentIntent.Currency,
                CreatedAt = paymentIntent.Created,
                CustomerEmail = paymentIntent.ReceiptEmail ?? string.Empty,
                ErrorMessage = paymentIntent.Status == "succeeded" ? string.Empty : "Payment not completed"
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResponseDto
            {
                IsSuccess = false,
                PaymentIntentId = paymentIntentId,
                Status = "error",
                Amount = 0,
                Currency = string.Empty,
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = string.Empty,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResponseDto> CancelPaymentAsync(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var cancelOptions = new PaymentIntentCancelOptions
            {
                CancellationReason = "requested_by_customer"
            };
            
            var paymentIntent = await service.CancelAsync(paymentIntentId, cancelOptions);

            return new PaymentResponseDto
            {
                IsSuccess = paymentIntent.Status == "canceled",
                PaymentIntentId = paymentIntent.Id,
                Status = paymentIntent.Status,
                Amount = paymentIntent.Amount,
                Currency = paymentIntent.Currency,
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = paymentIntent.ReceiptEmail ?? string.Empty,
                ErrorMessage = paymentIntent.Status == "canceled" ? string.Empty : "Failed to cancel payment"
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResponseDto
            {
                IsSuccess = false,
                PaymentIntentId = paymentIntentId,
                Status = "error",
                Amount = 0,
                Currency = string.Empty,
                CreatedAt = DateTime.UtcNow,
                CustomerEmail = string.Empty,
                ErrorMessage = ex.Message
            };
        }
    }
}

