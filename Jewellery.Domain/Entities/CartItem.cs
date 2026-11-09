using Jewellery.Domain.Common;
using System;

namespace Jewellery.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public int CartId { get; private set; }

        public int ProductId { get; private set; }

        public Product Product { get; private set; } = null!;

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        private CartItem()
        {
        }

        public CartItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (quantity > product.Quantity)
                throw new InvalidOperationException("Insufficient product stock.");

            Product = product;
            ProductId = product.Id;
            Quantity = quantity;
            UnitPrice = product.SellingPrice;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (quantity > Product.Quantity)
                throw new InvalidOperationException("Insufficient product stock.");

            Quantity = quantity;
            SetUpdatedAt();
        }
    }
}