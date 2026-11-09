
using Jewellery.Domain.Entities;

namespace Jewellery.Application.Interfaces.Repositories
{
    public interface IProductImageRepository
    {
        Task AddAsync(ProductImage image);

        Task AddRangeAsync(IEnumerable<ProductImage> images);
    }
}
