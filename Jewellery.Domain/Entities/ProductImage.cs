
using System;

namespace Jewellery.Domain.Entities
{
    public class ProductImage
    {
        public int Id { get; private set; }

        public int ProductId { get; private set; }

        public string ImageUrl { get; private set; } = string.Empty;

        public bool IsPrimary { get; private set; }

        public int DisplayOrder { get; private set; }

        public Product Product { get; private set; } = null!;

        private ProductImage()
        {
        }
        public void SetPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }

        public void UpdateDisplayOrder(int displayOrder)
        {
            if (displayOrder < 0)
                throw new ArgumentException("Invalid display order.");

            DisplayOrder = displayOrder;
        }

        public ProductImage(
            int productId,
            string imageUrl,
            bool isPrimary = false,
            int displayOrder = 0)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID.");

            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("Image URL is required.");

            ProductId = productId;
            ImageUrl = imageUrl;
            IsPrimary = isPrimary;
            DisplayOrder = displayOrder;
        }
    }
}
