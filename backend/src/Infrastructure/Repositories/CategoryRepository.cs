using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly InventoryContext _dbContext;
    public CategoryRepository(InventoryContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetCategoryWithProductsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
