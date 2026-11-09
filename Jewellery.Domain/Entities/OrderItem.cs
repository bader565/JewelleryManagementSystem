using Jewellery.Domain.Common;
using System;

namespace Jewellery.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; private set; }

        public int ProductId { get; private set; }

        public Product Product { get; private set; } = null!;

        public string ProductName { get; private set; } = string.Empty;

        public string? ProductImageUrl { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        private OrderItem()
        {
        }

        public OrderItem(
            Product product,
            int quantity,
            string? productImageUrl = null,
            decimal? unitPrice = null)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            if (quantity > product.Quantity)
                throw new InvalidOperationException(
                    "Insufficient product stock.");

            if (unitPrice.HasValue && unitPrice.Value < 0)
                throw new ArgumentException(
                    "Unit price cannot be negative.");

            ProductId = product.Id;
            Product = product;
            ProductName = product.Name;
            ProductImageUrl = productImageUrl;
            Quantity = quantity;

            // حفظ سعر المنتج وقت إضافته للسلة
            UnitPrice = unitPrice ?? product.SellingPrice;
        }
    }
}