using CetinBurger.Application.Contracts.Orders;

namespace CetinBurger.Application.Services;

public interface IOrderService
{
    Task<OrderDto> CheckoutAsync(string userId, CheckoutRequestDto request);
    Task<List<OrderDto>> GetMyOrdersAsync(string userId);
    Task<OrderDto?> GetByIdAsync(int id, string userId, bool isAdmin);
    Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    Task<List<AdminOrderListDto>> GetAllOrdersAsync();
    Task<bool> UpdateMailSentStatusAsync(int orderId, bool mailSentStatus);
    Task<bool> DeleteOrderAsync(int orderId);
}
