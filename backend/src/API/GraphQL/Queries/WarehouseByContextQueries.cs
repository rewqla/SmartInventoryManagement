using API.GraphQL.Filters;
using API.GraphQL.Queries.Models;
using API.GraphQL.Sorting;
using Application.DTO.Inventory;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.GraphQL.Queries;

[QueryType]
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

    public async Task<WarehouseModelDTO?> GetWarehouseByIdFromContext([Service] InventoryContext context, Guid id,
        CancellationToken cancellationToken)
    {
        return await context.Warehouses
            .Include(w => w.Inventories)
            .ThenInclude(i => i.Product)
            .Where(w => w.Id == id)
            .Select(w => new WarehouseModelDTO
            {
                Id = w.Id,
                Name = w.Name,
                Location = w.Location,
                Inventories = w.Inventories.Select(i => new InventoryModelDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name, // Assuming Product has a Name property
                    Quantity = i.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}