using Jewellery.Domain.Common;
using Jewellery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; private set; } = string.Empty;

        public string CustomerName { get; private set; } = string.Empty;

        public string? CustomerEmail { get; private set; }

        public string CustomerPhone { get; private set; } = string.Empty;

        public string ShippingAddress { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;

        public string? Notes { get; private set; }

        public OrderStatus Status { get; private set; }
            = OrderStatus.Pending;

        public ICollection<OrderItem> Items { get; private set; }
            = new List<OrderItem>();

        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

        private Order()
        {
        }

        public Order(
            string customerName,
            string customerPhone,
            string shippingAddress,
            string city,
            string? customerEmail = null,
            string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name is required.");

            if (string.IsNullOrWhiteSpace(customerPhone))
                throw new ArgumentException("Customer phone is required.");

            if (string.IsNullOrWhiteSpace(shippingAddress))
                throw new ArgumentException("Shipping address is required.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required.");

            OrderNumber = $"ORD-{Guid.NewGuid():N}"
                .Substring(0, 16).ToUpperInvariant();

            CustomerName = customerName;
            CustomerPhone = customerPhone;
            ShippingAddress = shippingAddress;
            City = city;
            CustomerEmail = customerEmail;
            Notes = notes;
            Status = OrderStatus.Pending;
        }

        public void AddItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Items.Add(item);
            SetUpdatedAt();
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending orders can be confirmed.");

            Status = OrderStatus.Confirmed;
            SetUpdatedAt();
        }

        public void Ship()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException(
                    "Only confirmed orders can be shipped.");

            Status = OrderStatus.Shipped;
            SetUpdatedAt();
        }

        public void Deliver()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException(
                    "Only shipped orders can be delivered.");

            Status = OrderStatus.Delivered;
            SetUpdatedAt();
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Shipped ||
                Status == OrderStatus.Delivered ||
                Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "This order cannot be cancelled.");
            }

            Status = OrderStatus.Cancelled;
            SetUpdatedAt();
        }
    }
}