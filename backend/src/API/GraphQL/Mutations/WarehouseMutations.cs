using API.GraphQL.Mappers.Warehouse;
using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Payload;
using API.GraphQL.Shared;
using API.GraphQL.Subscriptions;
using API.GraphQL.Subscriptions.EventsMessages;
using API.GraphQL.Subscriptions.Topics;
using Application.Exceptions;
using Application.Interfaces.Services.Warehouse;
using Application.Mapping.Errors;
using Application.Validation.Warehouse;
using FluentValidation;
using FluentValidation.Results;
using HotChocolate.Subscriptions;
using Infrastructure.Data;

namespace API.GraphQL.Mutations;

public sealed class WarehouseMutations
{
    [Error(typeof(ValidationException))]
    public async Task<CreateWarehousePayload> CreateWarehouse(IWarehouseService warehouseService,
        CreateWarehouseInput input, [Service] ITopicEventSender sender,
        WarehouseDTOValidator validator, CancellationToken cancellationToken)
    {
        var warehouseDTO = WarehouseMapper.ToDTO(input);

        var createdWarehouse =
            await warehouseService.CreateWarehouseAsync(warehouseDTO, cancellationToken);

        if (createdWarehouse.IsFailure)
        {
            var validationFailures = ValidationErrorMapper.MapToValidationFailures(createdWarehouse.Error.Errors);
            throw new ValidationException("Validation errors occurred", validationFailures);
        }

        var warehouseResult = WarehouseMapper.ToCreatePayload(createdWarehouse.Value!);
        warehouseDTO.Id = createdWarehouse.Value!.Id;

        await sender.SendAsync(nameof(WarehouseSubscriptions.WarehouseCreated), warehouseResult, cancellationToken);
        await sender.SendAsync(WarehouseTopics.Mutated,
            new WarehouseEventMessage(EventType.Created, warehouseDTO), cancellationToken);

        return warehouseResult;
    }

    [Error(typeof(InvalidGuidException))]
    [Error(typeof(ValidationException))]
    public async Task<UpdateWarehousePayload> UpdateWarehouse(IWarehouseService warehouseService,
        InventoryContext context, UpdateWarehouseInput input,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var warehouseDTO = WarehouseMapper.ToDTO(input);

        var updatedWarehouse = await warehouseService.UpdateWarehouseAsync(warehouseDTO, cancellationToken);

        if (updatedWarehouse.IsFailure)
        {
            if (updatedWarehouse.Error.Code == "Warehouse.ValidationError")
            {
                throw new ValidationException(updatedWarehouse.Error.Description);
            }

            if (updatedWarehouse.Error.Code == "Warehouse.NotFound")
            {
                throw new InvalidGuidException(updatedWarehouse.Error.Description);
            }
        }
        
        string updateWarehouseTopic = $"{updatedWarehouse.Value!.Id}_{nameof(WarehouseSubscriptions.WarehouseUpdated)}";
        await sender.SendAsync(updateWarehouseTopic, updatedWarehouse.Value, cancellationToken);

        await sender.SendAsync(WarehouseTopics.Mutated, new WarehouseEventMessage(EventType.Updated, warehouseDTO),
            cancellationToken);

        var warehouseResult = WarehouseMapper.ToUpdatePayload(updatedWarehouse.Value);

        return warehouseResult;
    }

    [Error(typeof(InvalidGuidException))]
    public async Task<bool> DeleteWarehouse(IWarehouseService warehouseService, Guid warehouseId,
        [Service] ITopicEventSender sender, CancellationToken cancellationToken)
    {
        var warehouseResult = await warehouseService.GetWarehouseByIdAsync(warehouseId, cancellationToken);

        if (warehouseResult.IsFailure)
        {
            throw new InvalidGuidException($"Warehouse with ID {warehouseId} not found.");
        }

        await warehouseService.DeleteWarehouse(warehouseId, cancellationToken);

        await sender.SendAsync(WarehouseTopics.Mutated,
            new WarehouseEventMessage(EventType.Deleted, warehouseResult.Value!), cancellationToken);

        return true;
    }
}