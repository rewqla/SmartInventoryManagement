using API.GraphQL.Mutations.Inputs;
using API.GraphQL.Mutations.Payload;
using Application.DTO.Warehouse;

namespace API.GraphQL.Mappers.Warehouse;

public static class WarehouseMapper
{
    public static WarehouseDTO ToDTO(CreateWarehouseInput warehouse)
    {
        if (warehouse == null)
            throw new ArgumentNullException(nameof(warehouse));

        return new WarehouseDTO
        {
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }
    
    public static CreateWarehousePayload ToPayload(WarehouseDTO warehouse)
    {
        if (warehouse == null)
            throw new ArgumentNullException(nameof(warehouse));

        return new CreateWarehousePayload
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }
}