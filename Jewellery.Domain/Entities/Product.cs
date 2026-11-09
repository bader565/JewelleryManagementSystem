
using Jewellery.Domain.Common;
using Jewellery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public string? Barcode { get; private set; }

        public GoldKarat Karat { get; private set; }

        public decimal Weight { get; private set; }

        public decimal PurchasePrice { get; private set; }

        public decimal SellingPrice { get; private set; }

        public int Quantity { get; private set; }

        // Product Images
        public ICollection<ProductImage> Images { get; private set; }
            = new List<ProductImage>();

        // Product Certificates
        public ICollection<Certificate> Certificates { get; private set; }
            = new List<Certificate>();
        // Product Invoices
        public ICollection<Invoice> Invoices { get; private set; }
            = new List<Invoice>();
        public ICollection<AppraisalReport> AppraisalReports { get; private set; }
    = new List<AppraisalReport>();
        private Product()
        {
        }
        public void AddAppraisalReport(AppraisalReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            AppraisalReports.Add(report);
        }
        public void AddInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            Invoices.Add(invoice);
        }
        public Product(
            string name,
            GoldKarat karat,
            decimal weight,
            decimal purchasePrice,
            decimal sellingPrice,
            int quantity,
            string? description = null,
            string? barcode = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.");

            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero.");

            if (purchasePrice < 0 || sellingPrice < 0)
                throw new ArgumentException("Prices cannot be negative.");

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Name = name;
            Karat = karat;
            Weight = weight;
            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;
            Quantity = quantity;
            Description = description;
            Barcode = barcode;
        }

        public void UpdatePrice(
            decimal purchasePrice,
            decimal sellingPrice)
        {
            if (purchasePrice < 0 || sellingPrice < 0)
                throw new ArgumentException("Invalid price.");

            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;

            SetUpdatedAt();
        }


        public void UpdateWeightAndKarat(
            decimal weight,
            GoldKarat karat)
        {
            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero.");

            if (!Enum.IsDefined(typeof(GoldKarat), karat))
                throw new ArgumentException("Invalid gold karat.");

            Weight = weight;
            Karat = karat;

            SetUpdatedAt();
        }


        public void AddImage(ProductImage image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            Images.Add(image);
        }

        public void RemoveImage(ProductImage image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            Images.Remove(image);
        }

        public void SetPrimaryImage(int imageId)
        {
            var image = Images.FirstOrDefault(i => i.Id == imageId);

            if (image == null)
                throw new ArgumentException("Image not found.");

            foreach (var item in Images)
                item.SetPrimary(item.Id == imageId);
        }

        public void AddCertificate(Certificate certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            Certificates.Add(certificate);
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (quantity > Quantity)
                throw new InvalidOperationException("Insufficient product stock.");

            Quantity -= quantity;

            SetUpdatedAt();
        }
        public void UpdateDetails(
            string name,
            GoldKarat karat,
            decimal weight,
            decimal purchasePrice,
            decimal sellingPrice,
            int quantity,
            string? description,
            string? barcode)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.");

            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than zero.");

            if (purchasePrice < 0 || sellingPrice < 0)
                throw new ArgumentException("Prices cannot be negative.");

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Name = name;
            Karat = karat;
            Weight = weight;
            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;
            Quantity = quantity;
            Description = description;
            Barcode = barcode;

            SetUpdatedAt();
        }
    }
}
