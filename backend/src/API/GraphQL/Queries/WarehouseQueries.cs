using API.GraphQL.Filters;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;

namespace API.GraphQL.Queries;

public sealed class WarehouseQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, MaxPageSize = 20, DefaultPageSize = 5)]
    [UseFiltering(typeof(WarehouseFilterType))]
    public async Task<IEnumerable<WarehouseDTO>> GetWarehouse(IWarehouseService warehouseService) =>
        await warehouseService.GetWarehousesAsync();

    public async Task<WarehouseDTO?> GetWarehouseById(IWarehouseService warehouseService, Guid id) =>
        await warehouseService.GetWarehouseByIdAsync(id);
}