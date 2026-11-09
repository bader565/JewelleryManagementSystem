using Jewellery.Application.DTOs.Documents;

namespace Jewellery.Application.Interfaces.Services.Documents
{
    public interface IInvoiceParserService
    {
        InvoiceDataDto Parse(string extractedText);
    }
}