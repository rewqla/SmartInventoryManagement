﻿using Application.Common;
using Application.DTO.Warehouse;

namespace Application.Interfaces.Services.Warehouse;

public interface IWarehouseService
{
    // #TODO: Update the method to return a Result<T>
    Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<WarehouseDTO>>> GetWarehousesAsync(CancellationToken cancellationToken = default);
    // #TODO: Update the method to return a Result<T>
    Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO warehouseDto,CancellationToken cancellationToken = default);
    // #TODO: Update the method to return a Result<T>
    Task<WarehouseDTO> UpdateWarehouseAsync(WarehouseDTO warehouseDto,CancellationToken cancellationToken = default);
    // #TODO: Update the method to return a Result<T>
    Task<bool> DeleteWarehouse(Guid id, CancellationToken cancellationToken = default);
}