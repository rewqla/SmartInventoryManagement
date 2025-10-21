using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
{
    private readonly InventoryContext _dbContext;
    public InventoryRepository(InventoryContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Inventories
            .AsNoTracking()
            .Where(i => i.WarehouseId == warehouseId)
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .OrderBy(i => i.Product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Inventory?> GetWithLogsAsync(Guid inventoryId, CancellationToken ct = default)
    {
        return await _dbContext.Inventories
            .Include(i => i.InventoryLogs)
            .ThenInclude(log => log.User)
            .Include(i => i.Product)
            .Include(i => i.Warehouse)
            .FirstOrDefaultAsync(i => i.Id == inventoryId, ct);
    }
}
