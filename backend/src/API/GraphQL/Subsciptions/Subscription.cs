using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Infrastructure.Entities;

namespace API.GraphQL.Subsciptions;

public class Subscription
{
    [Subscribe]
    public Warehouse WarehouseCreated([EventMessage] Warehouse warehouse) => warehouse;
    
    [Subscribe]
    public Warehouse WarehouseDeleted([EventMessage] Warehouse warehouse) => warehouse;
    
    [SubscribeAndResolve]
    public ValueTask<ISourceStream<Warehouse>> WarehouseUpdated(
        Guid warehouseId, 
        [Service] ITopicEventReceiver receiver)
    {
        string topic = $"{warehouseId}_{nameof(WarehouseUpdated)}";
        
        return receiver.SubscribeAsync<Warehouse>(topic);
    }
}