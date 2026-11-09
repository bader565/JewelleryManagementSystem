using Jewellery.Application.DTOs.Orders;

namespace Jewellery.Application.Interfaces.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(
            string sessionId,
            CheckoutDto checkout);

        Task<OrderDto?> GetByIdAsync(int id);

        Task<List<OrderDto>> GetAllAsync();
    }
}