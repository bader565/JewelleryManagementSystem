using Jewellery.Application.DTOs.Documents;
using System.Threading.Tasks;

namespace Jewellery.Application.Interfaces.Services
{
    public interface IAppraisalReportService
    {
        Task SaveAsync(int productId, AppraisalDataDto data);
    }
}