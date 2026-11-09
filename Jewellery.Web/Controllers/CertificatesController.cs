using Jewellery.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class CertificatesController : Controller
    {
        private readonly ICertificateQueryService _certificateQueryService;

        public CertificatesController(
            ICertificateQueryService certificateQueryService)
        {
            _certificateQueryService = certificateQueryService;
        }

        public async Task<IActionResult> Index()
        {
            var certificates = await _certificateQueryService.GetAllAsync();

            return View(certificates);
        }

        public async Task<IActionResult> Details(int id)
        {
            var certificate = await _certificateQueryService.GetByIdAsync(id);

            if (certificate == null)
                return NotFound();

            return View(certificate);
        }
    }
}