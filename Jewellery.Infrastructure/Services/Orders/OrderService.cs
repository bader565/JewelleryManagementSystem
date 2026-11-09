using Jewellery.Application.DTOs.Orders;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services.Orders;
using Jewellery.Domain.Entities;
using Jewellery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;

        public OrderService(
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            ApplicationDbContext context)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<OrderDto> CreateOrderAsync(
            string sessionId,
            CheckoutDto checkout)
        {
            var cart = await _cartRepository.GetBySessionIdAsync(sessionId);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Your cart is empty.");

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order(
                    checkout.CustomerName,
                    checkout.CustomerPhone,
                    checkout.ShippingAddress,
                    checkout.City,
                    checkout.CustomerEmail,
                    checkout.Notes);

                foreach (var cartItem in cart.Items)
                {
                    var product = cartItem.Product;

                    if (cartItem.Quantity > product.Quantity)
                    {
                        throw new InvalidOperationException(
                            $"Insufficient stock for {product.Name}.");
                    }

                    var imageUrl = product.Images
                        .FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                        ?? product.Images.FirstOrDefault()?.ImageUrl;

                    var orderItem = new OrderItem(
      product,
      cartItem.Quantity,
      imageUrl,
      cartItem.UnitPrice
  );
                    order.AddItem(orderItem);

                    product.DecreaseStock(cartItem.Quantity);
                }

                await _orderRepository.AddAsync(order);

                _context.CartItems.RemoveRange(cart.Items);
                cart.Clear();

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToDto(order);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            return order == null ? null : MapToDto(order);
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(MapToDto).ToList();
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                ShippingAddress = order.ShippingAddress,
                City = order.City,
                Notes = order.Notes,
                Status = order.Status,
                CreatedAt = order.CreatedAt,

                Items = order.Items.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductImageUrl = item.ProductImageUrl,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }
    }
}