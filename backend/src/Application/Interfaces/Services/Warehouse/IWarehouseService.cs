using Application.DTO;

namespace Application.Interfaces.Services.Warehouse;

public interface IWarehouseService
{
    Task<WarehouseDTO> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WarehouseDTO>> GetWarehousesAsync(CancellationToken cancellationToken = default);
}