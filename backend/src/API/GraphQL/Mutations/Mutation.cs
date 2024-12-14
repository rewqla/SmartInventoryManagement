using API.GraphQL.Errors;
using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Subsciptions;
using HotChocolate.Subscriptions;
using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL.Mutations;

public class Mutation
{
    public async Task<Warehouse> CreateWarehouse(InventoryContext context, CreateWarehouseInput input,
        [Service] ITopicEventSender sender)
    {
        var warehouse = new Warehouse
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Location = input.Location
        };

        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();

        await sender.SendAsync(nameof(Subscription.WarehouseCreated), warehouse);
        
        return warehouse;
    }
    
    [Error(typeof(InvalidGuidError))]
    public async Task<Warehouse?> UpdateWarehouse(InventoryContext context, 
        UpdateWarehouseInput input, [Service] ITopicEventSender sender)
    {
        var warehouse = await context.Warehouses.FindAsync(input.Id);

        if (warehouse == null)
        {
            throw new InvalidGuidError($"Warehouse {input.Id} not found");
        }

        warehouse.Name = input.Name;
        warehouse.Location = input.Location;

        await context.SaveChangesAsync();

        string updateWarehouseTopic = $"{warehouse.Id}_{nameof(Subscription.WarehouseUpdated)}";
        await sender.SendAsync(updateWarehouseTopic, warehouse);
        
        return warehouse;
    }

    public async Task<bool> DeleteWarehouse(InventoryContext context, Guid warehouseId,
        [Service] ITopicEventSender sender)
    {
        var warehouse = await context.Warehouses.FindAsync(warehouseId);

        if (warehouse == null)
        {
            return false;
        }

        context.Warehouses.Remove(warehouse);
        await context.SaveChangesAsync();

        await sender.SendAsync(nameof(Subscription.WarehouseDeleted), warehouse);
        
        return true;
    }
}