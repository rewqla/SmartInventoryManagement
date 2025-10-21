using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;
public interface IInventoryRepository : IGenericRepository<Inventory>
{
    Task<IEnumerable<Inventory>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<Inventory?> GetWithLogsAsync(Guid inventoryId, CancellationToken ct = default);
}
