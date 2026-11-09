namespace Jewellery.Application.DTOs.Documents
{
    public class OcrResultDto
    {
        public string ExtractedText { get; set; } = string.Empty;

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public CertificateDataDto? CertificateData { get; set; }

        public InvoiceDataDto? InvoiceData { get; set; }

        public AppraisalDataDto? AppraisalData { get; set; }
    }
}