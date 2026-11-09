using Jewellery.Application.Interfaces.Services;
using Jewellery.Web.ViewModels.Documents;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IOcrService _ocrService;
        private readonly IInvoiceService _invoiceService;
        private readonly IAppraisalReportService _appraisalReportService;
        private readonly IProductService _productService;
        private readonly ICertificateService _certificateService;

        public DocumentsController(
            IOcrService ocrService,
            IProductService productService,
            IInvoiceService invoiceService,
            IAppraisalReportService appraisalReportService,
            ICertificateService certificateService)
        {
            _ocrService = ocrService;
            _productService = productService;
            _appraisalReportService = appraisalReportService;
            _certificateService = certificateService;
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new DocumentsViewModel
            {
                Products = await _productService.GetAllAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExtractText(DocumentsViewModel model)
        {
            model.Products = await _productService.GetAllAsync();

            if (model.File == null || model.File.Length == 0)
            {
                ViewBag.Error = "Please select a document.";
                return View("Index", model);
            }

            if (model.ProductId <= 0)
            {
                ViewBag.Error = "Please select a product.";
                return View("Index", model);
            }

            var allowedExtensions = new[]
            {
                ".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff"
            };

            var extension = Path.GetExtension(model.File.FileName)
                .ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ViewBag.Error = "Only image files are allowed.";
                return View("Index", model);
            }

            using var stream = model.File.OpenReadStream();

            var result = await _ocrService.ExtractTextAsync(
            stream,
            model.File.FileName,
            model.DocumentType
        );

            model.Result = result;

            if (result.Success && result.CertificateData != null)
            {
                await _certificateService.SaveAsync(
                    model.ProductId,
                    result.CertificateData
                );

                ViewBag.Success = "Certificate saved successfully.";
            }

            if (result.Success && result.InvoiceData != null)
            {
                await _invoiceService.SaveAsync(
                    model.ProductId,
                    result.InvoiceData
                );

                ViewBag.Success = "Invoice saved successfully.";
            }
            if (result.Success && result.AppraisalData != null)
            {
                await _appraisalReportService.SaveAsync(
                    model.ProductId,
                    result.AppraisalData
                );

                ViewBag.Success = "Appraisal report saved successfully.";
            }
            return View("Index", model);
        }
    }
}