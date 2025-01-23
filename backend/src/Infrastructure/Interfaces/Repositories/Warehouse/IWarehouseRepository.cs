using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories.Warehouse;
// todo: add Product repository interface
public interface IWarehouseRepository : IGenericRepository<Entities.Warehouse>
{
    Task<IEnumerable<Entities.Warehouse>> GetWarehousesWithInventoriesAsync(CancellationToken cancellationToken = default);
}