using API.GraphQL.Mappers.Warehouse;
using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Payload;
using API.GraphQL.Shared;
using API.GraphQL.Subscriptions;
using API.GraphQL.Subscriptions.EventsMessages;
using API.GraphQL.Subscriptions.Topics;
using Application.Exceptions;
using Application.Interfaces.Services.Warehouse;
using Application.Validation.Warehouse;
using FluentValidation;
using HotChocolate.Subscriptions;
using Infrastructure.Data;

namespace API.GraphQL.Mutations;

public  sealed class WarehouseMutations
{
    [Error(typeof(ValidationException))]
    public async Task<CreateWarehousePayload> CreateWarehouse(IWarehouseService warehouseService,
        CreateWarehouseInput input, [Service] ITopicEventSender sender,
        WarehouseDTOValidator validator, CancellationToken cancellationToken)
    {
        var warehouseDTO = WarehouseMapper.ToDTO(input);

        var createdWarehouse =
            await warehouseService.CreateWarehouseAsync(warehouseDTO, cancellationToken);

        var warehouseResult = WarehouseMapper.ToCreatePayload(createdWarehouse);
        warehouseDTO.Id = createdWarehouse.Id;

        await sender.SendAsync(nameof(WarehouseSubscriptions.WarehouseCreated), warehouseResult, cancellationToken);
        await sender.SendAsync(WarehouseTopics.Mutated, 
            new WarehouseEventMessage(EventType.Created, warehouseDTO), cancellationToken);

        return warehouseResult;
    }

    [Error(typeof(InvalidGuidException))]
    public async Task<UpdateWarehousePayload> UpdateWarehouse(IWarehouseService warehouseService,
        InventoryContext context, UpdateWarehouseInput input,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var warehouseDTO = WarehouseMapper.ToDTO(input);

        var updatedWarehouse = await warehouseService.UpdateWarehouseAsync(warehouseDTO, cancellationToken);

        string updateWarehouseTopic = $"{updatedWarehouse.Id}_{nameof(WarehouseSubscriptions.WarehouseUpdated)}";
        await sender.SendAsync(updateWarehouseTopic, updatedWarehouse, cancellationToken);

        await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Updated, warehouseDTO),
            cancellationToken);

        var warehouseResult = WarehouseMapper.ToUpdatePayload(updatedWarehouse);

        return warehouseResult;
    }
    
    [Error(typeof(InvalidGuidException))]
    public async Task<bool> DeleteWarehouse(IWarehouseService warehouseService, Guid warehouseId,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var warehouseDTO = await warehouseService.GetWarehouseByIdAsync(warehouseId, cancellationToken);
    
        if (warehouseDTO == null)
        {
            throw new InvalidGuidException($"Warehouse with ID {warehouseId} not found.");
        }

        
        await warehouseService.DeleteWarehouse(warehouseId, cancellationToken);

        await sender.SendAsync(WarehouseTopics.Mutated,
            new WarehouseEventMessage(EventType.Deleted, warehouseDTO), cancellationToken);

        return true;
    }
}