
using Jewellery.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;

namespace Jewellery.Web.Models.Products
{
    public class UpdateProductViewModel : UpdateProductDto
    {
        public List<IFormFile> NewImages { get; set; } = new();
        public List<int> DeletedImageIds { get; set; } = new();
        public int? PrimaryImageId { get; set; }

     
        public List<ExistingImageViewModel> ExistingImages { get; set; } = new();
    }

    public class ExistingImageViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
    }
}
