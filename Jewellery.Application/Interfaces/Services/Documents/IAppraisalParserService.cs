using Jewellery.Application.DTOs.Documents;

namespace Jewellery.Application.Interfaces.Services.Documents
{
    public interface IAppraisalParserService
    {
        AppraisalDataDto Parse(string extractedText);
    }
}