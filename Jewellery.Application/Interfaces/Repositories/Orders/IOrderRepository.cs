using Jewellery.Domain.Entities;

namespace Jewellery.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);

        Task<Order?> GetByIdAsync(int id);

        Task<List<Order>> GetAllAsync();

        Task SaveChangesAsync();
    }
}