using Jewellery.Application.DTOs.Documents;

namespace Jewellery.Application.Interfaces.Services
{
    public interface ICertificateQueryService
    {
        Task<List<CertificateDto>> GetAllAsync();

        Task<CertificateDto?> GetByIdAsync(int id);
    }
}