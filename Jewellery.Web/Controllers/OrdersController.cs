using Jewellery.Application.Interfaces.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // عرض جميع الطلبات
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            return View(orders);
        }

        // تفاصيل الطلب
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}