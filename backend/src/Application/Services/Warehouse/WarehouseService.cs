using Application.DTO;
using Application.DTO.Warehouse;
using Application.Errors;
using Application.Interfaces.Services.Warehouse;
using Application.Mapping.Warehouse;
using Application.Validation.Warehouse;
using FluentValidation;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Microsoft.Extensions.Logging;

namespace Application.Services.Warehouse;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly WarehouseDTOValidator _warehouseDTOValidator;
    private readonly ILogger<WarehouseService> _logger;

    public WarehouseService(IWarehouseRepository warehouseRepository, ILogger<WarehouseService> logger,
        WarehouseDTOValidator warehouseDtoValidator)
    {
        _warehouseRepository = warehouseRepository;
        _logger = logger;
        _warehouseDTOValidator = warehouseDtoValidator;
    }

    public async Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieve warehouse with id: " + id);
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        if (warehouse == null)
        {
            _logger.LogError($"Warehouse with id {id} not found");
            throw new InvalidGuidError($"Warehouse {id} not found");
        }

        _logger.LogInformation("Warehouse with id " + id + " retrieved");
        return WarehouseMapper.ToDTO(warehouse);
    }

    public async Task<IEnumerable<WarehouseDTO>> GetWarehousesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieve all warehouses");
        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);

        return warehouses.Select(item => WarehouseMapper.ToDTO(item));
    }

    public async Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO warehouseDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _warehouseDTOValidator.ValidateAsync(warehouseDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        warehouseDto.Id = Guid.NewGuid();
        var warehouse = WarehouseMapper.ToEntity(warehouseDto);

        await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        await _warehouseRepository.CompleteAsync();

        return warehouseDto;
    }

    public async Task<WarehouseDTO> UpdateWarehouseAsync(WarehouseDTO warehouseDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _warehouseDTOValidator.ValidateAsync(warehouseDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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