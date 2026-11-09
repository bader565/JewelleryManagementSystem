
using Jewellery.Domain.Common;
using System;

namespace Jewellery.Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public int InvoiceId { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public int? Quantity { get; private set; }

        public decimal? UnitPrice { get; private set; }

        public decimal? TotalPrice { get; private set; }

        public Invoice Invoice { get; private set; } = null!;

        private InvoiceItem()
        {
        }

        public InvoiceItem(
            string description,
            int? quantity,
            decimal? unitPrice,
            decimal? totalPrice)
        {
            Description = description ?? string.Empty;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }
    }
}
