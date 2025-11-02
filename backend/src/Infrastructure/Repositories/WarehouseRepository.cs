using Infrastructure.Data;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WarehouseRepository : GenericRepository<Entities.Warehouse>, IWarehouseRepository
{
    private readonly InventoryContext _dbContext;

    public WarehouseRepository(InventoryContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Entities.Warehouse?> GetWarehouseWithInventoriesAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Warehouses
            .Where(w => w.Id == id)
            .Include(w => w.Inventories)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
