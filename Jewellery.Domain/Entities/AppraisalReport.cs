
namespace Jewellery.Domain.Entities
{
    public class AppraisalReport
    {
        public int Id { get; private set; }

        public int ProductId { get; private set; }
        public string ReportNumber { get; private set; } = string.Empty;
        public string AppraiserName { get; private set; } = string.Empty;
        public DateTime? AppraisalDate { get; private set; }

        public string ProductType { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Material { get; private set; } = string.Empty;
        public string Purity { get; private set; } = string.Empty;
        public decimal? Weight { get; private set; }

        public string DiamondShape { get; private set; } = string.Empty;
        public string CutGrade { get; private set; } = string.Empty;
        public string ColorGrade { get; private set; } = string.Empty;
        public string ClarityGrade { get; private set; } = string.Empty;
        public string Symmetry { get; private set; } = string.Empty;
        public string Fluorescence { get; private set; } = string.Empty;

        public decimal? EstimatedValue { get; private set; }
        public string Currency { get; private set; } = string.Empty;

        public Product Product { get; private set; } = null!;

        private AppraisalReport()
        {
        }

        public AppraisalReport(int productId,
            string reportNumber,
            string appraiserName,
            DateTime? appraisalDate,
            string productType,
            string description,
            string material,
            string purity,
            decimal? weight,
            string diamondShape,
            string cutGrade,
            string colorGrade,
            string clarityGrade,
            string symmetry,
            string fluorescence,
            decimal? estimatedValue,
            string currency)
        {
            ProductId = productId;
            ReportNumber = reportNumber;
            AppraiserName = appraiserName;
            AppraisalDate = appraisalDate;
            ProductType = productType;
            Description = description;
            Material = material;
            Purity = purity;
            Weight = weight;
            DiamondShape = diamondShape;
            CutGrade = cutGrade;
            ColorGrade = colorGrade;
            ClarityGrade = clarityGrade;
            Symmetry = symmetry;
            Fluorescence = fluorescence;
            EstimatedValue = estimatedValue;
            Currency = currency;
        }
    }
}
