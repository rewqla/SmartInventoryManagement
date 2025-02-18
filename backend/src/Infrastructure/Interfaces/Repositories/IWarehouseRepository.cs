using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;
public interface IWarehouseRepository : IGenericRepository<Entities.Warehouse>
{
    Task<IEnumerable<Entities.Warehouse>> GetWarehousesWithInventoriesAsync(CancellationToken cancellationToken = default);
}