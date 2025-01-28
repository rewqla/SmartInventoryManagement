using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Payload;
using Application.DTO.Warehouse;

namespace API.GraphQL.Mappers.Warehouse;

public static class WarehouseMapper
{
    public static WarehouseDTO ToDTO(CreateWarehouseInput warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new WarehouseDTO
        {
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }
    
    public static WarehouseDTO ToDTO(UpdateWarehouseInput warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new WarehouseDTO
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }
    
    public static CreateWarehousePayload ToCreatePayload(WarehouseDTO warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new CreateWarehousePayload(warehouse.Id, warehouse.Name, warehouse.Location);
    }
    
    public static UpdateWarehousePayload ToUpdatePayload(WarehouseDTO warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new UpdateWarehousePayload(warehouse.Id, warehouse.Name, warehouse.Location);
    }
}