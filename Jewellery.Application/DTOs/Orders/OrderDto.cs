using Jewellery.Domain.Enums;

namespace Jewellery.Application.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string? CustomerEmail { get; set; }

        public string CustomerPhone { get; set; } = string.Empty;

        public string ShippingAddress { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

        public DateTime CreatedAt { get; set; }
    }
}