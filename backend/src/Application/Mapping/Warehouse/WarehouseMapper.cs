using Application.DTO;
using Application.DTO.Warehouse;

namespace Application.Mapping.Warehouse;

public static class WarehouseMapper
{
    public static WarehouseDTO ToDTO(Infrastructure.Entities.Warehouse warehouse)
    {
        if (warehouse == null)
            throw new ArgumentNullException(nameof(warehouse));

        return new WarehouseDTO
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }

    public static Infrastructure.Entities.Warehouse ToEntity(WarehouseDTO warehouseDto)
    {
        if (warehouseDto == null)
            throw new ArgumentNullException(nameof(warehouseDto));

        return new Infrastructure.Entities.Warehouse
        {
            Id = warehouseDto.Id, 
            Name = warehouseDto.Name,
            Location = warehouseDto.Location
        };
    }
}