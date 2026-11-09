using System.ComponentModel.DataAnnotations;

namespace Jewellery.Application.DTOs.Orders
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? CustomerEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Shipping address is required.")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}