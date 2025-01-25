using API.GraphQL.Filters;
using API.GraphQL.Sorting;
using Application.DTO.Inventory;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public sealed class WarehouseByContextQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, MaxPageSize = 30, DefaultPageSize = 5)]
    [UseProjection]
    [UseFiltering(typeof(WarehouseFilterType))]
    [UseSorting(typeof(WarehouseSortType))]
    public IQueryable<Warehouse> GetWarehousesFromContext(
        [Service] InventoryContext context, 
        CancellationToken cancellationToken)
    {
        return context.Warehouses;
    }

    public async Task<Warehouse?> GetWarehouseByIdFromContext([Service] InventoryContext context, Guid id,
        CancellationToken cancellationToken)
    {
        return await context.Warehouses.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }
}