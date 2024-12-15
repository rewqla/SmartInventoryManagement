using API.GraphQL.Errors;
using API.GraphQL.Mappers.Warehouse;
using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Payload;
using API.GraphQL.Shared;
using API.GraphQL.Subscriptions;
using API.GraphQL.Subscriptions.EventsMessages;
using API.GraphQL.Subscriptions.Topics;
using Application.Interfaces.Services.Warehouse;
using HotChocolate.Subscriptions;
using Infrastructure.Data;
using Infrastructure.Entities;

namespace API.GraphQL.Mutations;

public class Mutation
{
    public async Task<CreateWarehousePayload> CreateWarehouse(IWarehouseService warehouseService,
        CreateWarehouseInput input,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var createdWarehouse =
            await warehouseService.CreateWarehouseAsync(WarehouseMapper.ToDTO(input), cancellationToken);

        var warehouseResult = WarehouseMapper.ToPayload(createdWarehouse);

        await sender.SendAsync(nameof(Subscription.WarehouseCreated), warehouseResult, cancellationToken);
        // await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Created, warehouseDto));

        return warehouseResult;
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

        await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Updated, warehouse));

        return warehouse;
    }

    public async Task<bool> DeleteWarehouse(IWarehouseService warehouseService, Guid warehouseId,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        await warehouseService.DeleteWarehouse(warehouseId, cancellationToken);

        // await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Deleted, warehouse));

        return true;
    }
}