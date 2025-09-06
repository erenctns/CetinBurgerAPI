using CetinBurger.Application.Contracts.Payments;
using CetinBurger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetinBurger.API.Controllers;


/// Ödeme işlemleri için controller

[ApiController]
[Route("api/[controller]")]
[Authorize] // Sadece giriş yapmış kullanıcılar
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IOrderService _orderService;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger, IConfiguration configuration, IOrderService orderService)
    {
        _paymentService = paymentService;
        _logger = logger;
        _configuration = configuration;
        _orderService = orderService;
    }
    /// Stripe ile ödeme işlemini gerçekleştir
    [HttpPost("process")]
    public async Task<ActionResult<PaymentResponseDto>> ProcessPayment([FromBody] PaymentRequestDto request)
    {
        try
        {
            _logger.LogInformation("Payment processing started for amount: {Amount} {Currency}", request.Amount, request.Currency);
            
            var result = await _paymentService.ProcessPaymentAsync(request);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Payment successful. PaymentIntent ID: {PaymentIntentId}", result.PaymentIntentId);
                
                // Eğer OrderId varsa, Order status'unu güncelle
                if (request.OrderId.HasValue)
                {
                    await UpdateOrderStatus(request.OrderId.Value, "Paid");
                }
                
                return Ok(result);
            }
            else
            {
                _logger.LogWarning("Payment failed. Error: {ErrorMessage}", result.ErrorMessage);
                
                // Eğer OrderId varsa, Order status'unu güncelle
                if (request.OrderId.HasValue)
                {
                    await UpdateOrderStatus(request.OrderId.Value, "PaymentFailed");
                }
                
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during payment processing");
            return StatusCode(500, new PaymentResponseDto
            {
                IsSuccess = false,
                ErrorMessage = "An unexpected error occurred during payment processing"
            });
        }
    }
    /// Order status'unu güncelle
    private async Task UpdateOrderStatus(int orderId, string status)
    {
        try
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (success)
            {
                _logger.LogInformation("Order {OrderId} status updated to {Status}", orderId, status);
            }
            else
            {
                _logger.LogWarning("Failed to update order {OrderId} status to {Status}", orderId, status);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status for {OrderId}", orderId);
        }
    }
    /// Ödeme durumunu kontrol et
    [HttpGet("status/{paymentIntentId}")]
    public async Task<ActionResult<PaymentResponseDto>> GetPaymentStatus(string paymentIntentId)
    {
        try
        {
            var result = await _paymentService.GetPaymentStatusAsync(paymentIntentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status for {PaymentIntentId}", paymentIntentId);
            return StatusCode(500, new PaymentResponseDto
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while retrieving payment status"
            });
        }
    }
    /// Ödeme işlemini iptal et
    [HttpPost("cancel/{paymentIntentId}")]
    public async Task<ActionResult<PaymentResponseDto>> CancelPayment(string paymentIntentId)
    {
        try
        {
            var result = await _paymentService.CancelPaymentAsync(paymentIntentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling payment for {PaymentIntentId}", paymentIntentId);
            return StatusCode(500, new PaymentResponseDto
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while canceling payment"
            });
        }
    }
    /// Test ödemesi için demo endpoint (sadece development'ta)
    [HttpPost("test")]
    [AllowAnonymous]
    public ActionResult<object> TestPayment()
    {
        try
        {
            // Stripe konfigürasyonunu kontrol et
            var stripeKey = _configuration["Stripe:SecretKey"];
            if (string.IsNullOrEmpty(stripeKey))
            {
                return BadRequest(new { error = "Stripe SecretKey not configured" });
            }

            return Ok(new
            {
                message = "Payment system is working!",
                stripeConfigured = !string.IsNullOrEmpty(stripeKey),
                usage = new
                {
                    endpoint = "POST /api/Payment/process",
                    examples = new
                    {
                        withTestCard = new
                        {
                            amount = 1447, // 14.47 TL (kuruş cinsinden)
                            currency = "try",
                            paymentMethodId = "", // Boş string - otomatik test kartı kullanılır
                            description = "Test ödeme",
                            orderId = 1,
                            customerEmail = "test@example.com"
                        },
                        withCustomCard = new
                        {
                            amount = 2500, // 25.00 TL (kuruş cinsinden)
                            currency = "try",
                            paymentMethodId = "pm_xxx", // Stripe'dan gelen PaymentMethod ID
                            description = "Özel kart ile ödeme",
                            orderId = 2,
                            customerEmail = "customer@example.com"
                        }
                    }
                },
                testCards = new
                {
                    success = "4242 4242 4242 4242",
                    declined = "4000 0000 0000 0002",
                    insufficient = "4000 0000 0000 9995"
                },
                note = "PaymentMethodId boş bırakılırsa otomatik olarak test kartı kullanılır. Amount kuruş cinsinden tam sayı olmalıdır (1 TL = 100 kuruş)."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in TestPayment endpoint");
            return StatusCode(500, new { error = "Internal server error", details = ex.Message });
        }
    }
}
