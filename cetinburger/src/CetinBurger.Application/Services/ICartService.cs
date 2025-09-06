using CetinBurger.Application.Contracts.Carts;

namespace CetinBurger.Application.Services;

public interface ICartService
{
    Task<CartDto> GetMyCartAsync(string userId);
    Task<CartDto> AddItemAsync(string userId, AddCartItemRequestDto request);
    Task<CartDto> UpdateItemAsync(string userId, int itemId, UpdateCartItemRequestDto request);
    Task<CartDto> RemoveItemAsync(string userId, int itemId);
}
