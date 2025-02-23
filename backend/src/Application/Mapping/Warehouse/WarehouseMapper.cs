using Application.DTO;
using Application.DTO.Inventory;
using Application.DTO.Warehouse;

namespace Application.Mapping.Warehouse;

public static class WarehouseMapper
{
    public static WarehouseDTO ToDTO(Infrastructure.Entities.Warehouse warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new WarehouseDTO
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location,
            // Inventories = warehouse.Inventories.Select(i => new InventoryDTO
            // {
            //     ProductId = i.ProductId,
            //     ProductName = i.Product.Name, 
            //     Quantity = i.Quantity
            // }).ToList()
        };
    }

    public static Infrastructure.Entities.Warehouse ToEntity(WarehouseDTO warehouseDto)
    {
        ArgumentNullException.ThrowIfNull(warehouseDto);

        return new Infrastructure.Entities.Warehouse
        {
            Id = warehouseDto.Id, 
            Name = warehouseDto.Name,
            Location = warehouseDto.Location
        };
    }
}