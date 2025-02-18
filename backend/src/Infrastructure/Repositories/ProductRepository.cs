using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly InventoryContext _dbContext;
    public ProductRepository(InventoryContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}