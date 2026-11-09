
using Jewellery.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;

namespace Jewellery.Web.Models.Products
{
    public class CreateProductViewModel : CreateProductDto
    {
        public List<IFormFile> Images { get; set; } = new();
    }
}
