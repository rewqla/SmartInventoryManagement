using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;

public interface ICategoryRepository: IGenericRepository<Category>
{
    Task<Category?> GetCategoryWithProductsAsync(Guid id, CancellationToken cancellationToken = default);
}
