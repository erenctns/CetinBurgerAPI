using CetinBurger.Application.Contracts.Orders;
using CetinBurger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CetinBurger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Customer,Admin")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderDto>> Checkout([FromBody] CheckoutRequestDto request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var order = await _orderService.CheckoutAsync(userId, request);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("Sepet boş"))
            {
                return BadRequest(ex.Message);
            }
            if (ex.Message.Contains("geçersiz/kapalı ürünler"))
            {
                return Conflict(ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    //kullanıcının kendi siparişlerinin listesini döner OrderDTO tipinde.
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> MyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = await _orderService.GetMyOrdersAsync(userId);
        return Ok(orders);
    }

    //verilen siparişlerin tümünden istediğimiz siparişi seçmek için olan yer.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById([FromRoute] int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdmin = User.IsInRole("Admin");
            var order = await _orderService.GetByIdAsync(id, userId, isAdmin);
            
            if (order == null)
            {
                return NotFound();
            }
            
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("erişim yetkiniz yok"))
            {
                return Forbid();
            }
            return BadRequest(ex.Message);
        }
    }

    // Admin: Tüm siparişleri getir
    [HttpGet("admin/all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<AdminOrderListDto>>> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch
        {
            return StatusCode(500, "Siparişler getirilirken bir hata oluştu.");
        }
    }

    // Admin: Mail gönderim durumunu güncelle
    [HttpPut("{id:int}/mailStatus")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateMailSentStatus([FromRoute] int id, [FromBody] UpdateMailSentStatusRequestDto request)
    {
        try
        {
            var success = await _orderService.UpdateMailSentStatusAsync(id, request.MailSentStatus);
            
            if (!success)
            {
                return NotFound("Sipariş bulunamadı.");
            }
            
            return Ok(new { message = "Mail gönderim durumu başarıyla güncellendi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Mail gönderim durumu güncellenirken bir hata oluştu: {ex.Message}");
        }
    }

    // Admin: Sipariş silme
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteOrder([FromRoute] int id)
    {
        try
        {
            var success = await _orderService.DeleteOrderAsync(id);
            
            if (!success)
            {
                return NotFound(new { message = "Sipariş bulunamadı." });
            }

            return Ok(new { message = "Sipariş başarıyla silindi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Sipariş silinirken bir hata oluştu: {ex.Message}");
        }
    }
}


