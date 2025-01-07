using Application.Common;
using Application.DTO.Warehouse;

namespace Application.Interfaces.Services.Warehouse;

public interface IWarehouseService
{
    Task<Result<WarehouseDTO>> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<WarehouseDTO>>> GetWarehousesAsync(CancellationToken cancellationToken = default);
    Task<Result<WarehouseDTO>> CreateWarehouseAsync(WarehouseDTO warehouseDto,CancellationToken cancellationToken = default);
    Task<Result<WarehouseDTO>> UpdateWarehouseAsync(WarehouseDTO warehouseDto,CancellationToken cancellationToken = default);
    // #TODO: Update the method to return a Result<T>
    Task<Result<bool>> DeleteWarehouse(Guid id, CancellationToken cancellationToken = default);
}