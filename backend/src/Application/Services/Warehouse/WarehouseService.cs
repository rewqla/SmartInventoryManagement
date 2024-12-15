using Application.DTO;
using Application.Interfaces.Services.Warehouse;
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

        return new WarehouseDTO
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }

    public async Task<IEnumerable<WarehouseDTO>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);

        return warehouses.Select(item => new WarehouseDTO
        {
            Id = item.Id,
            Name = item.Name,
            Location = item.Location
        });
    }
}