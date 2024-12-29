using API.GraphQL.Mutations.Payload;
using API.GraphQL.Subscriptions.EventsMessages;
using API.GraphQL.Subscriptions.Topics;
using Application.DTO.Warehouse;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Infrastructure.Entities;

namespace API.GraphQL.Subscriptions;

public sealed class WarehouseSubscriptions
{
    [Subscribe]
    public CreateWarehousePayload WarehouseCreated([EventMessage] CreateWarehousePayload createWarehouse) => createWarehouse;
    
    [SubscribeAndResolve]
    public ValueTask<ISourceStream<UpdateWarehousePayload>> WarehouseUpdated(
        Guid warehouseId, 
        [Service] ITopicEventReceiver receiver)
    {
        string topic = $"{warehouseId}_{nameof(WarehouseUpdated)}";
        
        return receiver.SubscribeAsync<UpdateWarehousePayload>(topic);
    }
    
    [Subscribe]
    [Topic(WarehouseTopics.Mutated)]
    public WarehouseEventMessage WarehouseMutated([EventMessage] WarehouseEventMessage warehouseEventMessage) => warehouseEventMessage;
}