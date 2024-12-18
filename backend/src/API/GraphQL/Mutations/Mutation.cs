using API.GraphQL.Mappers.Warehouse;
using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Payload;
using API.GraphQL.Subscriptions;
using Application.Errors;
using Application.Interfaces.Services.Warehouse;
using Application.Validation.Warehouse;
using FluentValidation;
using HotChocolate.Subscriptions;
using Infrastructure.Data;

namespace API.GraphQL.Mutations;

public class Mutation
{
    [Error(typeof(ValidationException))]
    public async Task<CreateWarehousePayload> CreateWarehouse(IWarehouseService warehouseService,
        CreateWarehouseInput input, [Service] ITopicEventSender sender,
        WarehouseDTOValidator validator, CancellationToken cancellationToken)
    {
        var warehouseDTO = WarehouseMapper.ToDTO(input);
        
        var validationResult = await validator.ValidateAsync(warehouseDTO, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var createdWarehouse =
            await warehouseService.CreateWarehouseAsync(warehouseDTO, cancellationToken);

        var warehouseResult = WarehouseMapper.ToCreatePayload(createdWarehouse);

        await sender.SendAsync(nameof(Subscription.WarehouseCreated), warehouseResult, cancellationToken);
        // await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Created, warehouseDto));

        return warehouseResult;
    }

    [Error(typeof(InvalidGuidError))]
    public async Task<UpdateWarehousePayload> UpdateWarehouse(IWarehouseService warehouseService,
        InventoryContext context, UpdateWarehouseInput input,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var warehouseDTO = WarehouseMapper.ToDTO(input);

        var updatedWarehouse = await warehouseService.UpdateWarehouseAsync(warehouseDTO, cancellationToken);

        // string updateWarehouseTopic = $"{updatedWarehouse.Id}_{nameof(Subscription.WarehouseUpdated)}";
        // await sender.SendAsync(updateWarehouseTopic, warehouse);
        //
        // await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Updated, warehouse));

        var warehouseResult = WarehouseMapper.ToUpdatePayload(updatedWarehouse);
        
        return warehouseResult;
    }

    public async Task<bool> DeleteWarehouse(IWarehouseService warehouseService, Guid warehouseId,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        await warehouseService.DeleteWarehouse(warehouseId, cancellationToken);

        // await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Deleted, warehouse));

        return true;
    }
}