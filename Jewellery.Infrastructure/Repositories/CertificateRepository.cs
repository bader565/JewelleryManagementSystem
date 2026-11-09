using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Domain.Entities;
using Jewellery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.Infrastructure.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Certificate>> GetAllAsync()
        {
            return await _context.Certificates
                .Include(c => c.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Certificate?> GetByIdAsync(int id)
        {
            return await _context.Certificates
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}