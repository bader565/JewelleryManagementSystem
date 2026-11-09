
using Jewellery.Application.DTOs.Products;
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Jewellery.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;

        public ProductService(
            IProductRepository productRepository,
            IFileStorageService fileStorageService)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
        }

        // Get all products
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Barcode = p.Barcode,
                Karat = p.Karat,
                Weight = p.Weight,
                PurchasePrice = p.PurchasePrice,
                SellingPrice = p.SellingPrice,
                Quantity = p.Quantity,

                ImageUrls = p.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageUrl)
                    .ToList()

            }).ToList();
        }

        // Get product by ID
        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Barcode = product.Barcode,
                Karat = product.Karat,
                Weight = product.Weight,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                Quantity = product.Quantity,

                ImagesDetails = product.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        IsPrimary = i.IsPrimary,
                        DisplayOrder = i.DisplayOrder
                    })
                    .ToList(),

                ImageUrls = product.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageUrl)
                    .ToList()
            };
        }

        // Create product with images
        public async Task CreateAsync(
            CreateProductDto dto,
            List<IFormFile> images)
        {
            var product = new Product(
                dto.Name,
                dto.Karat,
                dto.Weight,
                dto.PurchasePrice,
                dto.SellingPrice,
                dto.Quantity,
                dto.Description,
                dto.Barcode);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            var savedFiles = new List<string>();

            try
            {
                for (int i = 0; i < images.Count; i++)
                {
                    var file = images[i];

                    await using var stream = file.OpenReadStream();

                    var imageUrl = await _fileStorageService.SaveFileAsync(
                        stream,
                        file.FileName,
                        file.ContentType);

                    savedFiles.Add(imageUrl);

                    product.AddImage(new ProductImage(
                        product.Id,
                        imageUrl,
                        isPrimary: i == 0,
                        displayOrder: i));
                }

                if (images.Count > 0)
                    await _productRepository.SaveChangesAsync();
            }
            catch
            {
                foreach (var file in savedFiles)
                    await _fileStorageService.DeleteFileAsync(file);

                throw;
            }
        }

        // Update product and images
        public async Task UpdateAsync(
            UpdateProductDto dto,
            List<IFormFile> newImages,
            List<int> deletedImageIds,
            int? primaryImageId)
        {
            var product = await _productRepository.GetByIdAsync(dto.Id);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            product.UpdateDetails(
                dto.Name,
                dto.Karat,
                dto.Weight,
                dto.PurchasePrice,
                dto.SellingPrice,
                dto.Quantity,
                dto.Description,
                dto.Barcode);

            var savedFiles = new List<string>();
            var deletedFiles = new List<string>();

            try
            {
                foreach (var imageId in deletedImageIds.Distinct())
                {
                    var image = product.Images
                        .FirstOrDefault(i => i.Id == imageId);

                    if (image == null)
                        continue;

                    deletedFiles.Add(image.ImageUrl);
                    product.RemoveImage(image);
                }

                foreach (var file in newImages.Where(f => f.Length > 0))
                {
                    await using var stream = file.OpenReadStream();

                    var imageUrl = await _fileStorageService.SaveFileAsync(
                        stream,
                        file.FileName,
                        file.ContentType);

                    savedFiles.Add(imageUrl);

                    product.AddImage(new ProductImage(
                        product.Id,
                        imageUrl,
                        displayOrder: product.Images.Count));
                }

                if (primaryImageId.HasValue)
                {
                    product.SetPrimaryImage(primaryImageId.Value);
                }
                else if (product.Images.Any() &&
                         !product.Images.Any(i => i.IsPrimary))
                {
                    product.SetPrimaryImage(product.Images.First().Id);
                }

                await _productRepository.SaveChangesAsync();

                foreach (var file in deletedFiles)
                    await _fileStorageService.DeleteFileAsync(file);
            }
            catch
            {
                foreach (var file in savedFiles)
                    await _fileStorageService.DeleteFileAsync(file);

                throw;
            }
        }

        // Delete product and its images
        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var imageUrls = product.Images
                .Select(i => i.ImageUrl)
                .ToList();

            await _productRepository.DeleteAsync(product);
            await _productRepository.SaveChangesAsync();

            foreach (var imageUrl in imageUrls)
                await _fileStorageService.DeleteFileAsync(imageUrl);
        }
    }
}
