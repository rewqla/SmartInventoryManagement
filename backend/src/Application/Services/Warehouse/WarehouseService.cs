using Application.DTO;
using Application.DTO.Warehouse;
using Application.Errors;
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

    public async Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        return WarehouseMapper.ToDTO(warehouse);
    }

    public async Task<IEnumerable<WarehouseDTO>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);

        return warehouses.Select(item => WarehouseMapper.ToDTO(item));
    }

    public async Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO warehouseDto,
        CancellationToken cancellationToken = default)
    {
        warehouseDto.Id = Guid.NewGuid();
        var warehouse = WarehouseMapper.ToEntity(warehouseDto);

        await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        await _warehouseRepository.CompleteAsync();

        return warehouseDto;
    }

    public async Task<WarehouseDTO> UpdateWarehouseAsync(WarehouseDTO warehouseDto,
        CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(warehouseDto.Id, cancellationToken);
        
        if (warehouse == null)
        {
            throw new InvalidGuidError($"Warehouse {warehouseDto.Id} not found");
        }
        
        warehouse.Name = warehouseDto.Name;
        warehouse.Location = warehouseDto.Location;
        
        _warehouseRepository.Update(warehouse);
        await _warehouseRepository.CompleteAsync();

        return warehouseDto;
    }

    public async Task<bool> DeleteWarehouse(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        if (warehouse == null) return false;

        _warehouseRepository.Delete(warehouse);
        await _warehouseRepository.CompleteAsync();

        return true;
    }
}