﻿using Application.Common;
using Application.DTO;
using Application.DTO.Warehouse;
using Application.Exceptions;
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

    public async Task<Result<WarehouseDTO>> GetWarehouseByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieve warehouse with id: " + id);
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        if (warehouse == null)
        {
            _logger.LogError($"Warehouse with id {id} not found");
            return Result<WarehouseDTO>.Failure(CommonErrors.NotFound("warehouse", id));
        }

        _logger.LogInformation("Warehouse with id " + id + " retrieved");

        var warehouseDto = WarehouseMapper.ToDTO(warehouse);
        return Result<WarehouseDTO>.Success(warehouseDto);
    }

    public async Task<Result<IEnumerable<WarehouseDTO>>> GetWarehousesAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieve all warehouses");

        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);
        var warehousesDto = warehouses.Select(WarehouseMapper.ToDTO);

        return Result<IEnumerable<WarehouseDTO>>.Success(warehousesDto);
    }

    public async Task<Result<WarehouseDTO>> CreateWarehouseAsync(WarehouseDTO warehouseDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _warehouseDTOValidator.ValidateAsync(warehouseDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorDetails = validationResult.Errors.Select(error => new  ErrorDetail
            {
                PropertyName = error.PropertyName,
                ErrorMessage = error.ErrorMessage
            }).ToList();
            
            return Result<WarehouseDTO>.Failure(CommonErrors.ValidationError("warehouse", errorDetails));
        }

        warehouseDto.Id = Guid.NewGuid();
        var warehouse = WarehouseMapper.ToEntity(warehouseDto);

        // todo: add try catch to handle possible errors
        await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        await _warehouseRepository.CompleteAsync();

        return Result<WarehouseDTO>.Success(warehouseDto);
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
            throw new InvalidGuidException($"Warehouse {warehouseDto.Id} not found");
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