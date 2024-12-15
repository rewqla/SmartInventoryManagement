using Application.DTO;
using Application.DTO.Warehouse;

namespace Application.Interfaces.Services.Warehouse;

public interface IWarehouseService
{
    Task<WarehouseDTO> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WarehouseDTO>> GetWarehousesAsync(CancellationToken cancellationToken = default);
    Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO warehouseDto,CancellationToken cancellationToken = default);
    Task<bool> DeleteWarehouse(Guid id, CancellationToken cancellationToken = default);
}