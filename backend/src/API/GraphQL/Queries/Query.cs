using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;

namespace API.GraphQL;

public class Query
{
    public async Task<IEnumerable<Warehouse>> GetWarehouse(IWarehouseRepository warehouseRepository) =>
        await warehouseRepository.GetAllAsync();

    public async Task<Warehouse?> GetWarehouseById(IWarehouseRepository warehouseRepository, Guid id) =>
        await warehouseRepository.FindByIdAsync(id);
}