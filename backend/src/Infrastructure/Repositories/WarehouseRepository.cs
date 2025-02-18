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

    public async Task<IEnumerable<Entities.Warehouse>> GetWarehousesWithInventoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Warehouses
            .Include(w => w.Inventories)
            .ThenInclude(p=>p.Product)
            .ToListAsync(cancellationToken);
    }
}