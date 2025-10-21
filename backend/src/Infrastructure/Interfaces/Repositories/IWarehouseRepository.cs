using Infrastructure.Interfaces.Repositories.Base;

namespace Infrastructure.Interfaces.Repositories;
public interface IWarehouseRepository : IGenericRepository<Entities.Warehouse>
{
    Task<Entities.Warehouse?> GetWarehouseWithInventoriesAsync(Guid id, CancellationToken cancellationToken = default);
}
