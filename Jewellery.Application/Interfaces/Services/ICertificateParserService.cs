
using Jewellery.Application.DTOs.Documents;

namespace Jewellery.Application.Interfaces.Services
{
    public interface ICertificateParserService
    {
        CertificateDataDto Parse(string extractedText);
    }
}
