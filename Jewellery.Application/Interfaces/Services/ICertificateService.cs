using Jewellery.Application.DTOs.Documents;
using System.Threading.Tasks;

namespace Jewellery.Application.Interfaces.Services
{
    public interface ICertificateService
    {
        Task SaveAsync(int productId, CertificateDataDto data);
    }
}