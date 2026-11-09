
using Jewellery.Domain.Common;
using System;

namespace Jewellery.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        public int ProductId { get; private set; }

        public string CertificateNumber { get; private set; } = string.Empty;

        public string ProductType { get; private set; } = string.Empty;

        public decimal? Weight { get; private set; }

        public string Purity { get; private set; } = string.Empty;

        public decimal? GoldPercentage { get; private set; }

        public decimal? SilverPercentage { get; private set; }

        public decimal? CopperPercentage { get; private set; }

        public decimal? OtherPercentage { get; private set; }

        public Product Product { get; private set; } = null!;

        private Certificate()
        {
        }

        public Certificate(
            int productId,
            string certificateNumber,
            string productType,
            decimal? weight,
            string purity,
            decimal? goldPercentage,
            decimal? silverPercentage,
            decimal? copperPercentage,
            decimal? otherPercentage)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID.");

            ProductId = productId;
            CertificateNumber = certificateNumber ?? string.Empty;
            ProductType = productType ?? string.Empty;
            Weight = weight;
            Purity = purity ?? string.Empty;
            GoldPercentage = goldPercentage;
            SilverPercentage = silverPercentage;
            CopperPercentage = copperPercentage;
            OtherPercentage = otherPercentage;
        }
    }
}
