
using Jewellery.Domain.Common;
using System;
using System.Collections.Generic;

namespace Jewellery.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public int ProductId { get; private set; }

        public string InvoiceNumber { get; private set; } = string.Empty;
        public DateTime? InvoiceDate { get; private set; }

        public string SellerName { get; private set; } = string.Empty;
        public string SellerAddress { get; private set; } = string.Empty;

        public string CustomerName { get; private set; } = string.Empty;
        public string BillingAddress { get; private set; } = string.Empty;
        public string DeliveryAddress { get; private set; } = string.Empty;

        public decimal? SubTotal { get; private set; }
        public decimal? TaxAmount { get; private set; }
        public decimal? TotalAmount { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public Product Product { get; private set; } = null!;
        public ICollection<InvoiceItem> Items { get; private set; }
    = new List<InvoiceItem>();

        private Invoice()
        {
        }

        public void AddItem(InvoiceItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Items.Add(item);
            SetUpdatedAt();
        }

        public Invoice(
            int productId,
            string invoiceNumber,
            DateTime? invoiceDate,
            string sellerName,
            string sellerAddress,
            string customerName,
            string billingAddress,
            string deliveryAddress,
            decimal? subTotal,
            decimal? taxAmount,
            decimal? totalAmount,
            string currency)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID.");

            ProductId = productId;
            InvoiceNumber = invoiceNumber ?? string.Empty;
            InvoiceDate = invoiceDate;
            SellerName = sellerName ?? string.Empty;
            SellerAddress = sellerAddress ?? string.Empty;
            CustomerName = customerName ?? string.Empty;
            BillingAddress = billingAddress ?? string.Empty;
            DeliveryAddress = deliveryAddress ?? string.Empty;
            SubTotal = subTotal;
            TaxAmount = taxAmount;
            TotalAmount = totalAmount;
            Currency = currency ?? string.Empty;
        }
    }
}
