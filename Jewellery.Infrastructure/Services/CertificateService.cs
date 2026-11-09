using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Domain.Entities;

namespace Jewellery.Infrastructure.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IProductRepository _productRepository;

        public CertificateService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task SaveAsync(int productId, CertificateDataDto data)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new Exception("Product not found.");

            var certificate = new Certificate(
                productId,
                data.CertificateNumber,
                data.ProductType,
                data.Weight,
                data.Purity,
                data.GoldPercentage,
                data.SilverPercentage,
                data.CopperPercentage,
                data.OtherPercentage
            );

            product.AddCertificate(certificate);

            await _productRepository.SaveChangesAsync();
        }
    }
}