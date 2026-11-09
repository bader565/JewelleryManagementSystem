using Jewellery.Domain.Entities;

namespace Jewellery.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetBySessionIdAsync(string sessionId);

        Task AddAsync(Cart cart);

        Task SaveChangesAsync();
    }
}