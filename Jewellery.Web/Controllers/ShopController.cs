using Jewellery.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Shop
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();

        
            var availableProducts = products
                .Where(p => p.Quantity > 0)
                .ToList();

            return View(availableProducts);
        }
    }
}