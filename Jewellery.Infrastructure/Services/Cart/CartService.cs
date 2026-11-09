using Jewellery.Application.DTOs.Cart;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services.Cart;
using Jewellery.Domain.Entities;
using Jewellery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.Infrastructure.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationDbContext _context;

        public CartService(
            ICartRepository cartRepository,
            ApplicationDbContext context)
        {
            _cartRepository = cartRepository;
            _context = context;
        }

        public async Task<CartDto> GetCartAsync(string sessionId)
        {
            var cart = await _cartRepository.GetBySessionIdAsync(sessionId);

            if (cart == null)
                return new CartDto();

            return MapToDto(cart);
        }

        public async Task AddToCartAsync(
            string sessionId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var cart = await _cartRepository.GetBySessionIdAsync(sessionId);

            if (cart == null)
            {
                cart = new Jewellery.Domain.Entities.Cart(sessionId);
                await _cartRepository.AddAsync(cart);
            }

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new InvalidOperationException("Product not found.");

            cart.AddItem(product, quantity);

            await _cartRepository.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(
            string sessionId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var cart = await _cartRepository.GetBySessionIdAsync(sessionId);

            if (cart == null)
                throw new InvalidOperationException("Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
                throw new InvalidOperationException("Cart item not found.");

            item.UpdateQuantity(quantity);

            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(string sessionId, int productId)
        {
            var cart = await _cartRepository.GetBySessionIdAsync(sessionId);

            if (cart == null)
                return;

            cart.RemoveItem(productId);

            await _cartRepository.SaveChangesAsync();
        }

        private static CartDto MapToDto(Jewellery.Domain.Entities.Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ImageUrl = item.Product.Images
                        .FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                        ?? item.Product.Images.FirstOrDefault()?.ImageUrl,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}