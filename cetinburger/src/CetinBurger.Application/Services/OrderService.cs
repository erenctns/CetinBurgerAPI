using AutoMapper;
using CetinBurger.Application.Contracts.Orders;
using CetinBurger.Domain.Entities;
using CetinBurger.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CetinBurger.Application.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // Sepetten sipariş oluştur (ödeme ayrı adım)
    public async Task<OrderDto> CheckoutAsync(string userId, CheckoutRequestDto request)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Sepet boş. Sipariş oluşturulamadı.");
        }

        // Ürün uygunluk kontrolü
        var unavailable = cart.Items.Where(i => i.Product == null || !i.Product.IsAvailable).ToList();
        if (unavailable.Count > 0)
        {
            throw new InvalidOperationException("Sepette geçersiz/kapalı ürünler var. Lütfen güncelleyin.");
        }

        using var tx = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // AutoMapper ile Order oluştur
            var order = _mapper.Map<Order>(request);
            order.UserId = userId;
            order.Status = "Pending"; // Ödeme yapılmadan önce "Pending" olmalı,stripe ödemesinden sonra duruma göre değişicek.
            order.CreatedAt = DateTime.UtcNow;
            
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            decimal total = 0m;
            // Sepetteki her bir ürün için OrderItem oluştur
            foreach (var ci in cart.Items)
            {
                var oi = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product!.Name,
                    UnitPrice = ci.UnitPrice,
                    Quantity = ci.Quantity,
                    CreatedAt = DateTime.UtcNow
                };
                total += oi.UnitPrice * oi.Quantity;
                _dbContext.OrderItems.Add(oi);
            }
            order.TotalAmount = total;
            await _dbContext.SaveChangesAsync();

            // Sepeti temizle
            _dbContext.CartItems.RemoveRange(cart.Items);
            await _dbContext.SaveChangesAsync();

            // Transaction'ı commit et - Order "Pending" olarak kalır
            await tx.CommitAsync();

            // OrderDto dön
            var created = await _dbContext.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .FirstAsync(o => o.Id == order.Id);
            return _mapper.Map<OrderDto>(created);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    // Kullanıcının kendi siparişlerini getir
    public async Task<List<OrderDto>> GetMyOrdersAsync(string userId)
    {
        var orders = await _dbContext.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        return _mapper.Map<List<OrderDto>>(orders);
    }

    // Sipariş detayını getir (Admin tüm siparişlere erişebilir)
    public async Task<OrderDto?> GetByIdAsync(int id, string userId, bool isAdmin)
    {
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
        {
            return null;
        }
        
        if (!isAdmin && order.UserId != userId)
        {
            throw new InvalidOperationException("Bu siparişe erişim yetkiniz yok.");
        }
        
        return _mapper.Map<OrderDto>(order);
    }

    // Sipariş durumunu güncelle
    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _dbContext.Orders.FindAsync(orderId);
        if (order == null)
        {
            return false;
        }

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // Admin: Tüm siparişleri optimize edilmiş DTO ile getir
    public async Task<List<AdminOrderListDto>> GetAllOrdersAsync()
    {
        var orders = await _dbContext.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        // User bilgilerini ayrı sorgu ile al
        var userIds = orders.Select(o => o.UserId).Distinct().ToList();
        var users = await _dbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u);

        // AutoMapper ile Order -> AdminOrderListDto dönüşümü
        var adminOrders = _mapper.Map<List<AdminOrderListDto>>(orders);

        // Sadece UserEmail'i set et
        for (int i = 0; i < orders.Count; i++)
        {
            if (users.TryGetValue(orders[i].UserId, out var user))
            {
                adminOrders[i].UserEmail = user.Email ?? "";
            }
        }

        return adminOrders;
    }

    // Mail gönderim durumunu güncelle (Admin için)
    public async Task<bool> UpdateMailSentStatusAsync(int orderId, bool mailSentStatus)
    {
        var order = await _dbContext.Orders.FindAsync(orderId);
        if (order == null)
        {
            return false;
        }

        order.MailSentStatus = mailSentStatus;
        order.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return false;
        }

        // Önce sipariş öğelerini sil
        _dbContext.OrderItems.RemoveRange(order.Items);
        
        // Sonra siparişi sil
        _dbContext.Orders.Remove(order);
        
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
