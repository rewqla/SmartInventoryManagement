﻿using API.GraphQL.Filters;
using API.GraphQL.Sorting;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;

namespace API.GraphQL.Queries;

public sealed class WarehouseQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, MaxPageSize = 20, DefaultPageSize = 5)]
    [UseFiltering(typeof(WarehouseFilterType))]
    [UseSorting(typeof(WarehouseSortType))]
    // todo: add projection
    public async Task<IEnumerable<WarehouseDTO>> GetWarehouse(IWarehouseService warehouseService,
        CancellationToken cancellationToken)
    {
        var result = await warehouseService.GetWarehousesAsync(cancellationToken);

        return result.Value!;
    }

    public async Task<WarehouseDTO?> GetWarehouseById(IWarehouseService warehouseService, Guid id,
        CancellationToken cancellationToken)
    {
        var result = await warehouseService.GetWarehouseByIdAsync(id, cancellationToken);

        return result.Value!;
    }
}