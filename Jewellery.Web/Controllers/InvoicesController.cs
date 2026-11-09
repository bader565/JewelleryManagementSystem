
using Jewellery.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: Invoices
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllAsync();

            return View(invoices);
        }

        // GET: Invoices/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }
    }
}
