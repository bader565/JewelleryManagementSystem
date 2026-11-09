using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Domain.Entities;
using Jewellery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Jewellery.Infrastructure.Repositories.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Jewellery.Domain.Entities.Cart?> GetBySessionIdAsync(
            string sessionId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task AddAsync(Jewellery.Domain.Entities.Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}