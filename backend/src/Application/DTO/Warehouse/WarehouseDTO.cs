﻿using Application.DTO.Inventory;

namespace Application.DTO.Warehouse;

public class WarehouseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public IEnumerable<InventoryDTO> Inventories { get; set; } = new List<InventoryDTO>();
}