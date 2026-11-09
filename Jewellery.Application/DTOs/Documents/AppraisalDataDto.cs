namespace Jewellery.Application.DTOs.Documents
{
    public class AppraisalDataDto
    {
        // Report Information
        public string ReportNumber { get; set; } = string.Empty;
        public string AppraiserName { get; set; } = string.Empty;
        public DateTime? AppraisalDate { get; set; }

        // Product Information
        public string ProductType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string Purity { get; set; } = string.Empty;
        public decimal? Weight { get; set; }

       
        public string DiamondShape { get; set; } = string.Empty;
        public string CutGrade { get; set; } = string.Empty;
        public string ColorGrade { get; set; } = string.Empty;
        public string ClarityGrade { get; set; } = string.Empty;
        public string Symmetry { get; set; } = string.Empty;
        public string Fluorescence { get; set; } = string.Empty;

        // Valuation
        public decimal? EstimatedValue { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}