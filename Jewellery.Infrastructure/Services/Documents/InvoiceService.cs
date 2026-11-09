
using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Domain.Entities;

namespace Jewellery.Infrastructure.Services.Document
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IProductRepository _productRepository;

        public InvoiceService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task SaveAsync(int productId, InvoiceDataDto data)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new Exception("Product not found.");

            var invoice = new Invoice(
                productId,
                data.InvoiceNumber,
                data.InvoiceDate,
                data.SellerName,
                data.SellerAddress,
                data.CustomerName,
                data.BillingAddress,
                data.DeliveryAddress,
                data.SubTotal,
                data.TaxAmount,
                data.TotalAmount,
                data.Currency
            );

            if (data.Items != null)
            {
                foreach (var item in data.Items)
                {
                    invoice.AddItem(new InvoiceItem(
                        item.Description,
                        item.Quantity,
                        item.UnitPrice,
                        item.TotalPrice
                    ));
                }
            }

            product.AddInvoice(invoice);

            await _productRepository.SaveChangesAsync();
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            return await _productRepository.GetAllInvoicesAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _productRepository.GetInvoiceByIdAsync(id);
        }
    }
}
