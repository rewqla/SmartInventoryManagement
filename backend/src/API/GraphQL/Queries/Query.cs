using Application.DTO;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;

namespace API.GraphQL.Queries;

public class Query
{
    [UseOffsetPaging]
    public async Task<IEnumerable<WarehouseDTO>> GetWarehouse(IWarehouseService warehouseService) =>
        await warehouseService.GetWarehousesAsync();

    public async Task<WarehouseDTO?> GetWarehouseById(IWarehouseService warehouseService, Guid id) =>
        await warehouseService.GetWarehouseByIdAsync(id);
}