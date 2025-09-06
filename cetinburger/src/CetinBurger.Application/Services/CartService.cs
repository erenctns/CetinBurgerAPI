using AutoMapper;
using CetinBurger.Application.Contracts.Carts;
using CetinBurger.Domain.Entities;
using CetinBurger.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CetinBurger.Application.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CartService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CartDto> GetMyCartAsync(string userId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            // Sepeti oluşturmadan AutoMapper ile boş görünüm döndür
            var empty = new Cart { Items = new List<CartItem>() };
            return _mapper.Map<CartDto>(empty);
        }

        // Geçersiz ürünleri filtrele (silme, sadece gösterme)
        var validItems = cart.Items.Where(i => i.Product != null && i.Product.IsAvailable).ToList();
        var invalidItems = cart.Items.Where(i => i.Product == null || !i.Product.IsAvailable).ToList();

        // Geçersiz ürünler için ProductName'i güncelle
        foreach (var item in invalidItems)
        {
            if (item.Product == null)
            {
                item.Product = new Product { Name = "Bu ürün artık geçerli değil" };
            }
            else
            {
                item.Product.Name = "Bu ürün artık geçerli değil";
            }
        }

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> AddItemAsync(string userId, AddCartItemRequestDto request)
    {
        var cart = await EnsureCartAsync(userId);

        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsAvailable);
        if (product is null)
        {
            throw new InvalidOperationException($"Product {request.ProductId} not found or unavailable.");
        }

        var existingItem = await _dbContext.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == request.ProductId);
        if (existingItem is null)
        {
            var newItem = _mapper.Map<CartItem>(request);
            newItem.CartId = cart.Id;
            newItem.ProductId = product.Id;
            newItem.UnitPrice = product.Price;
            newItem.CreatedAt = DateTime.UtcNow;
            _dbContext.CartItems.Add(newItem);
        }
        else
        {
            // Miktar güncellenirken istek DTO'sunu entity'ye uygula
            _mapper.Map(new UpdateCartItemRequestDto { Quantity = existingItem.Quantity + request.Quantity }, existingItem);
            existingItem.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
        return await ReturnCartDto(cart.Id);
    }

    public async Task<CartDto> UpdateItemAsync(string userId, int itemId, UpdateCartItemRequestDto request)
    {
        var cart = await EnsureCartAsync(userId);

        var item = await _dbContext.CartItems.FirstOrDefaultAsync(i => i.Id == itemId && i.CartId == cart.Id);
        if (item is null)
        {
            throw new InvalidOperationException($"Cart item {itemId} not found.");
        }

        _mapper.Map(request, item);
        item.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return await ReturnCartDto(cart.Id);
    }

    public async Task<CartDto> RemoveItemAsync(string userId, int itemId)
    {
        var cart = await EnsureCartAsync(userId);

        var item = await _dbContext.CartItems.FirstOrDefaultAsync(i => i.Id == itemId && i.CartId == cart.Id);
        if (item is null)
        {
            throw new InvalidOperationException($"Cart item {itemId} not found.");
        }

        _dbContext.CartItems.Remove(item);
        await _dbContext.SaveChangesAsync();
        return await ReturnCartDto(cart.Id);
    }

    private async Task<Cart> EnsureCartAsync(string userId)
    {
        var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart is null)
        {
            cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
            _dbContext.Carts.Add(cart);
            await _dbContext.SaveChangesAsync();
        }
        return cart;
    }

//sepetin içindeki ürünlerin bilgilerini cartdto tipinde döndürür.
    private async Task<CartDto> ReturnCartDto(int cartId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstAsync(c => c.Id == cartId);
        return _mapper.Map<CartDto>(cart);
    }
}
