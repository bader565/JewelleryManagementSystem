using Jewellery.Application.Interfaces.Services.Cart;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync(GetCartSessionId());

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCartAsync(
                    GetCartSessionId(), productId, quantity);

                TempData["Success"] = "Product added to cart successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            try
            {
                await _cartService.UpdateQuantityAsync(
                    GetCartSessionId(), productId, quantity);

                TempData["Success"] = "Cart updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            await _cartService.RemoveItemAsync(
                GetCartSessionId(), productId);

            TempData["Success"] = "Product removed from cart.";

            return RedirectToAction(nameof(Index));
        }
    }
}