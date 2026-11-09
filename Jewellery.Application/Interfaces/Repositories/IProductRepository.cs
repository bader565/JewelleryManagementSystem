
using Jewellery.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jewellery.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);

        Task<bool> SaveChangesAsync();

        Task<List<Invoice>> GetAllInvoicesAsync();

        Task<Invoice?> GetInvoiceByIdAsync(int id);
    }
}
