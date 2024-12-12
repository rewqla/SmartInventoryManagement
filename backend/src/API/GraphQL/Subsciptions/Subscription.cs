using Infrastructure.Entities;

namespace API.GraphQL.Subsciptions;

public class Subscription
{
    [Subscribe]
    public Warehouse WarehouseCreated([EventMessage] Warehouse warehouse) => warehouse;
}