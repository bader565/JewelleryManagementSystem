using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.DTOs.Products;
using Jewellery.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Jewellery.Web.ViewModels.Documents
{
    public class DocumentsViewModel
    {
        public IFormFile? File { get; set; }

        public int ProductId { get; set; }

        public DocumentType DocumentType { get; set; }
            = DocumentType.Certificate;

        public List<ProductDto> Products { get; set; } = new();

        public OcrResultDto? Result { get; set; }
    }
}