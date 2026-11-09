using Jewellery.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string SessionId { get; private set; } = string.Empty;

        public ICollection<CartItem> Items { get; private set; }
            = new List<CartItem>();

        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);

        private Cart()
        {
        }

        public Cart(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("Session ID is required.");

            SessionId = sessionId;
        }

        public void AddItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existingItem = Items.FirstOrDefault(
                i => i.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
                return;
            }

            Items.Add(new CartItem(product, quantity));
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(
                i => i.ProductId == productId);

            if (item != null)
                Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}