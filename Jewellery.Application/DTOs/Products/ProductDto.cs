
using Jewellery.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Jewellery.Application.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Barcode { get; set; }

        public GoldKarat Karat { get; set; }

        public decimal Weight { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new();
        public List<ProductImageDto> ImagesDetails { get; set; } = new();

    }
}
