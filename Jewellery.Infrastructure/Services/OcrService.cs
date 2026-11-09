
using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Application.Interfaces.Services.Documents;
using Jewellery.Domain.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using TesseractOCR;
using TesseractOCR.Enums;

namespace Jewellery.Infrastructure.Services
{
    public class OcrService : IOcrService
    {
        private readonly ICertificateParserService _certificateParser;
        private readonly IInvoiceParserService _invoiceParser;
        private readonly IAppraisalParserService _appraisalParser;

        public OcrService(
            ICertificateParserService certificateParser,
            IInvoiceParserService invoiceParser,
            IAppraisalParserService appraisalParser)
        {
            _certificateParser = certificateParser;
            _invoiceParser = invoiceParser;
            _appraisalParser = appraisalParser;
        }

        public async Task<OcrResultDto> ExtractTextAsync(
            Stream fileStream,
            string fileName,
            DocumentType documentType = DocumentType.Certificate)
        {
            string? tempFilePath = null;
            string? processedFilePath = null;

            try
            {
                if (fileStream == null || !fileStream.CanRead)
                {
                    return new OcrResultDto
                    {
                        Success = false,
                        ErrorMessage = "Invalid file stream."
                    };
                }

                tempFilePath = Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}{Path.GetExtension(fileName)}"
                );

                await using (var outputStream = File.Create(tempFilePath))
                {
                    await fileStream.CopyToAsync(outputStream);
                }

                // تحسين الصورة قبل OCR
                processedFilePath = Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}_processed.png"
                );

                using (var sourceImage =
                    SixLabors.ImageSharp.Image.Load(tempFilePath))
                {
                    int newWidth = sourceImage.Width * 2;
                    int newHeight = sourceImage.Height * 2;

                    sourceImage.Mutate(x => x
                        .Resize(newWidth, newHeight)
                        .Grayscale()
                        .Contrast(1.5f)
                        .GaussianSharpen()
                    );

                    await sourceImage.SaveAsync(
                        processedFilePath,
                        new PngEncoder()
                    );
                }

                string basePath = AppContext.BaseDirectory;

                string tessDataPath = Path.Combine(
                    basePath,
                    "tessdata"
                );

                string nativeDll = Path.Combine(
                    basePath,
                    "tesseract55.dll"
                );

                string leptonicaDll = Path.Combine(
                    basePath,
                    "leptonica-1.85.0.dll"
                );

                if (!Directory.Exists(tessDataPath))
                    throw new DirectoryNotFoundException(
                        $"Tessdata not found: {tessDataPath}");

                if (!File.Exists(Path.Combine(
                    tessDataPath, "eng.traineddata")))
                    throw new FileNotFoundException(
                        "English traineddata not found.");

                if (!File.Exists(nativeDll))
                    throw new FileNotFoundException(
                        $"Tesseract native DLL not found: {nativeDll}");

                if (!File.Exists(leptonicaDll))
                    throw new FileNotFoundException(
                        $"Leptonica DLL not found: {leptonicaDll}");

                if (!Environment.Is64BitProcess)
                    throw new Exception(
                        "Application is not running as 64-bit.");

                using var engine = new Engine(
                    tessDataPath,
                    "eng",
                    EngineMode.Default
                );

                using var image =
                    TesseractOCR.Pix.Image.LoadFromFile(
                        processedFilePath
                    );

                using var page = engine.Process(image);

                string extractedText = page.Text;

                var result = new OcrResultDto
                {
                    Success = true,
                    ExtractedText = extractedText
                };

                switch (documentType)
                {
                    case DocumentType.Certificate:
                        result.CertificateData =
                            _certificateParser.Parse(extractedText);
                        break;

                    case DocumentType.Invoice:
                        result.InvoiceData =
                            _invoiceParser.Parse(extractedText);
                        break;

                    case DocumentType.AppraisalReport:
                        result.AppraisalData =
                            _appraisalParser.Parse(extractedText);
                        break;

                    default:
                        throw new ArgumentException(
                            "Unsupported document type.");
                }

                return result;
            }
            catch (Exception ex)
            {
                return new OcrResultDto
                {
                    Success = false,
                    ErrorMessage = ex.ToString()
                };
            }
            finally
            {
                if (tempFilePath != null &&
                    File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                if (processedFilePath != null &&
                    File.Exists(processedFilePath))
                {
                    File.Delete(processedFilePath);
                }
            }
        }
    }
}
