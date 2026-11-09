using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services;

namespace Jewellery.Infrastructure.Services
{
    public class CertificateQueryService : ICertificateQueryService
    {
        private readonly ICertificateRepository _certificateRepository;

        public CertificateQueryService(
            ICertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        public async Task<List<CertificateDto>> GetAllAsync()
        {
            var certificates = await _certificateRepository.GetAllAsync();

            return certificates.Select(c => new CertificateDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                CertificateNumber = c.CertificateNumber,
                ProductType = c.ProductType,
                Weight = c.Weight,
                Purity = c.Purity,
                GoldPercentage = c.GoldPercentage,
                SilverPercentage = c.SilverPercentage,
                CopperPercentage = c.CopperPercentage,
                OtherPercentage = c.OtherPercentage,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task<CertificateDto?> GetByIdAsync(int id)
        {
            var c = await _certificateRepository.GetByIdAsync(id);

            if (c == null)
                return null;

            return new CertificateDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                CertificateNumber = c.CertificateNumber,
                ProductType = c.ProductType,
                Weight = c.Weight,
                Purity = c.Purity,
                GoldPercentage = c.GoldPercentage,
                SilverPercentage = c.SilverPercentage,
                CopperPercentage = c.CopperPercentage,
                OtherPercentage = c.OtherPercentage,
                CreatedAt = c.CreatedAt
            };
        }
    }
}