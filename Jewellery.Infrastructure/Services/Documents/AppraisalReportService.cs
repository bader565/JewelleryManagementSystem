
using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Domain.Entities;

namespace Jewellery.Infrastructure.Services.Documents
{
    public class AppraisalReportService : IAppraisalReportService
    {
        private readonly IProductRepository _productRepository;

        public AppraisalReportService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task SaveAsync(int productId, AppraisalDataDto data)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new Exception("Product not found.");

            var report = new AppraisalReport(
                productId,
                data.ReportNumber,
                data.AppraiserName,
                data.AppraisalDate,
                data.ProductType,
                data.Description,
                data.Material,
                data.Purity,
                data.Weight,
                data.DiamondShape,
                data.CutGrade,
                data.ColorGrade,
                data.ClarityGrade,
                data.Symmetry,
                data.Fluorescence,
                data.EstimatedValue,
                data.Currency
            );

            product.AddAppraisalReport(report);

            await _productRepository.SaveChangesAsync();
        }
    }
}
