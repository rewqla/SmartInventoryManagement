using API.GraphQL.Inputs;
using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL.Mutation;

public class Mutation
{
    public async Task<Warehouse> CreateWarehouse(InventoryContext context, WarehouseInput input)
    {
        var warehouse = new Warehouse
        {
            Id = new Guid(),
            Name = input.Name,
            Location = input.Location
        };
        
        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();
        
        return warehouse;
    }
}