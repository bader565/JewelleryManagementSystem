using Jewellery.Domain.Enums;

namespace Jewellery.Application.DTOs.Products
{
    public class UpdateProductDto
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
    }
}