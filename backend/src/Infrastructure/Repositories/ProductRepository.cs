using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly InventoryContext _dbContext;

    public ProductRepository(InventoryContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllWithCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> FindByIdWithCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryWithCategoryAsync(Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }
}
