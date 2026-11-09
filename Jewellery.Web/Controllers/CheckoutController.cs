using Jewellery.Application.DTOs.Orders;
using Jewellery.Application.Interfaces.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;

        public CheckoutController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private string GetCartSessionId()
        {
            var sessionId = HttpContext.Session.GetString("CartSessionId");

            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString("N");
                HttpContext.Session.SetString("CartSessionId", sessionId);
            }

            return sessionId;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CheckoutDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutDto checkout)
        {
            if (!ModelState.IsValid)
                return View(checkout);

            try
            {
                var order = await _orderService.CreateOrderAsync(
                    GetCartSessionId(), checkout);

                return RedirectToAction(
                    nameof(Confirmation),
                    new { id = order.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(checkout);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}