using Jewellery.Application.DTOs.Cart;

namespace Jewellery.Application.Interfaces.Services.Cart
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string sessionId);

        Task AddToCartAsync(string sessionId, int productId, int quantity);

        Task UpdateQuantityAsync(string sessionId, int productId, int quantity);

        Task RemoveItemAsync(string sessionId, int productId);
    }
}