using CetinBurger.Application.Contracts.Carts;
using CetinBurger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CetinBurger.API.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize(Roles = "Customer,Admin")] // Hem müşteri hem admin sepet kullanabilir
/// Kullanıcıların sepet işlemlerini (listeleme, ekleme, güncelleme, silme) yönetir.
/// Kimlik doğrulaması zorunludur ve kullanıcı, kimlik bilgisinden (ClaimTypes.NameIdentifier) alınır.

public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    /// Oturumdaki kullanıcının sepetini döner. Eğer sepet yoksa oluşturur ve döner.
    
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetMyCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var cart = await _cartService.GetMyCartAsync(userId);
        return Ok(cart);
    }

    /// Sepete ürün ekler. Aynı ürün zaten varsa miktarı arttırır.
    /// Ürün aktif (IsAvailable) olmalıdır.
    [HttpPost("items")] // Sepete ürün ekle
    public async Task<ActionResult<CartDto>> AddItem([FromBody] AddCartItemRequestDto request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var cart = await _cartService.AddItemAsync(userId, request);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// Sepetteki belirli bir satırın miktarını günceller.
    [HttpPut("items/{itemId:int}")] // Sepet öğesini güncelle (miktar)
    public async Task<ActionResult<CartDto>> UpdateItem([FromRoute] int itemId, [FromBody] UpdateCartItemRequestDto request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var cart = await _cartService.UpdateItemAsync(userId, itemId, request);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// Sepetten bir satırı kaldırır.
    [HttpDelete("items/{itemId:int}")] // Sepet öğesini sil
    public async Task<ActionResult<CartDto>> RemoveItem([FromRoute] int itemId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var cart = await _cartService.RemoveItemAsync(userId, itemId);
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}


