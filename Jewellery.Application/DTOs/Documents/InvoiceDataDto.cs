namespace Jewellery.Application.DTOs.Documents
{
    public class InvoiceDataDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime? InvoiceDate { get; set; }

        public string SellerName { get; set; } = string.Empty;
        public string SellerAddress { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;

        public decimal? SubTotal { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public List<InvoiceItemDataDto> Items { get; set; } = new();
    }

    public class InvoiceItemDataDto
    {
        public string Description { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}