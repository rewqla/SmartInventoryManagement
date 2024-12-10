using API.GraphQL.Errors;
using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Results;
using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL.Mutations;

public class Mutation
{
    public async Task<WarehouseResult> CreateWarehouse(InventoryContext context, CreateWarehouseInput input)
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
    
    public async Task<WarehouseResult?> UpdateWarehouse(InventoryContext context, 
        UpdateWarehouseInput input)
    {
        var warehouse = await context.Warehouses.FindAsync(input.Id);

        if (warehouse == null)
        {
            throw new InvalidGuidError($"Warehouse {input.Id} not found");
        }

        warehouse.Name = input.Name;
        warehouse.Location = input.Location;

        await context.SaveChangesAsync();

        return new WarehouseResult(warehouse.Id, warehouse.Name, warehouse.Location);
    }

    public async Task<bool> DeleteWarehouse(InventoryContext context, Guid warehouseId)
    {
        var warehouse = await context.Warehouses.FindAsync(warehouseId);

        if (warehouse == null)
        {
            return false;
        }

        context.Warehouses.Remove(warehouse);
        await context.SaveChangesAsync();

        return true;
    }
}