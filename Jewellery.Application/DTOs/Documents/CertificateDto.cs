namespace Jewellery.Application.DTOs.Documents
{
    public class CertificateDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string CertificateNumber { get; set; } = string.Empty;

        public string ProductType { get; set; } = string.Empty;

        public decimal? Weight { get; set; }

        public string Purity { get; set; } = string.Empty;

        public decimal? GoldPercentage { get; set; }

        public decimal? SilverPercentage { get; set; }

        public decimal? CopperPercentage { get; set; }

        public decimal? OtherPercentage { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}