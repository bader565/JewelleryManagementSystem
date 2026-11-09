
using Jewellery.Application.DTOs.Documents;
using Jewellery.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jewellery.Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task SaveAsync(int productId, InvoiceDataDto invoiceData);

        Task<List<Invoice>> GetAllAsync();

        Task<Invoice?> GetByIdAsync(int id);
    }
}
