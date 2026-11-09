
using Jewellery.Application.DTOs.Products;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Web.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Jewellery.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();

            return View(products);
        }

        // GET: Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateProductViewModel());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateProductViewModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _productService.CreateAsync(dto, dto.Images);

                TempData["Success"] = "Product added successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while saving the product.");

                return View(dto);
            }
        }

        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var vm = new UpdateProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Barcode = product.Barcode,
                Karat = product.Karat,
                Weight = product.Weight,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                Quantity = product.Quantity
            };

            vm.ExistingImages = product.ImagesDetails
       .Select(img => new ExistingImageViewModel
       {
           Id = img.Id,
           Url = img.ImageUrl
       })
       .ToList();

            return View(vm);
        }

        // POST: Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            UpdateProductViewModel dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                var product = await _productService.GetByIdAsync(id);

                if (product == null)
                    return NotFound();

                return View(dto);
            }

            try
            {
                await _productService.UpdateAsync(
                    dto,
                    dto.NewImages,
                    dto.DeletedImageIds,
                    dto.PrimaryImageId);

                TempData["Success"] = "Product updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "An error occurred while updating the product.");

                return View(dto);
            }
        }

        // GET: Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);

                TempData["Success"] = "Product deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
