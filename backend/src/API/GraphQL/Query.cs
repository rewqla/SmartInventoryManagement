using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL;

public class Query
{
    public List<Warehouse> GetWarehouse(InventoryContext context) =>  context.Warehouses.ToList();
    public async Task<Warehouse> GetWarehouseById(InventoryContext context, Guid id) => await  context.Warehouses.FindAsync(id);
}