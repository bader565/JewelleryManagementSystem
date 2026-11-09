
using Jewellery.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;

namespace Jewellery.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        private readonly string[] _allowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(
            Stream fileStream,
            string fileName,
            string contentType)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid image extension.");

            if (fileStream.Length == 0 || fileStream.Length > 5 * 1024 * 1024)
                throw new ArgumentException("Image must be between 1 byte and 5 MB.");

            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "products");

            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var output = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(output);
            }

            return $"/uploads/products/{uniqueFileName}";
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.CompletedTask;

            var fileName = Path.GetFileName(filePath);

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "products",
                fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
