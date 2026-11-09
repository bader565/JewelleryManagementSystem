
using Jewellery.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;

namespace Jewellery.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(int id);

        Task CreateAsync(
            CreateProductDto dto,
            List<IFormFile> images);

        Task UpdateAsync(
            UpdateProductDto dto,
            List<IFormFile> newImages,
            List<int> deletedImageIds,
            int? primaryImageId);

        Task DeleteAsync(int id);
    }
}
