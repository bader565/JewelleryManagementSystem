using Jewellery.Application.DTOs.Documents;
using Jewellery.Domain.Enums;
using System.IO;
using System.Threading.Tasks;

namespace Jewellery.Application.Interfaces.Services
{
    public interface IOcrService
    {
        Task<OcrResultDto> ExtractTextAsync(
            Stream fileStream,
            string fileName,
            DocumentType documentType = DocumentType.Certificate);
    }
}