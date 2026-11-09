using Jewellery.Domain.Entities;

namespace Jewellery.Application.Interfaces.Repositories
{
    public interface ICertificateRepository
    {
        Task<List<Certificate>> GetAllAsync();

        Task<Certificate?> GetByIdAsync(int id);
    }
}