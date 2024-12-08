using API.GraphQL.Mutation.Inputs;
using API.GraphQL.Mutation.Results;
using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL.Mutation;

public class Mutation
{
    public async Task<WarehouseResult> CreateWarehouse(InventoryContext context, WarehouseInput input)
    {
        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Location = input.Location
        };
        
        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();
        
        return new WarehouseResult(warehouse.Id, warehouse.Name, warehouse.Location);
    }
}