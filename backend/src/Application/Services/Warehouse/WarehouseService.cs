using Application.DTO;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Application.Mapping.Warehouse;
using Infrastructure.Interfaces.Repositories.Warehouse;

namespace Application.Services.Warehouse;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseDTO> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        return WarehouseMapper.ToDTO(warehouse);
    }

    public async Task<IEnumerable<WarehouseDTO>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);

        return warehouses.Select(item => WarehouseMapper.ToDTO(item));
    }
}