using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Product?> FindByIdWithCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryWithCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);

}
