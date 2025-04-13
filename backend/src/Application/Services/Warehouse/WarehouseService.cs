using Application.Common;
using Application.DTO;
using Application.DTO.Warehouse;
using Application.Exceptions;
using Application.Interfaces.Services.Report;
using Application.Interfaces.Services.Warehouse;
using Application.Mapping.Warehouse;
using Application.Validation;
using Application.Validation.Warehouse;
using FluentValidation;
using Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Services.Warehouse;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly WarehouseDTOValidator _warehouseDTOValidator;
    private readonly ILogger<WarehouseService> _logger;
    private readonly IReportService _reportService;

    public WarehouseService(IWarehouseRepository warehouseRepository, ILogger<WarehouseService> logger,
        WarehouseDTOValidator warehouseDtoValidator, IReportService reportService)
    {
        _warehouseRepository = warehouseRepository;
        _logger = logger;
        _warehouseDTOValidator = warehouseDtoValidator;
        _reportService = reportService;
    }

    public async Task<Result<WarehouseDTO>> GetWarehouseByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieve warehouse with id: " + id);
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        if (warehouse == null)
        {
            _logger.LogError($"Warehouse with id {id} not found");
            return Result<WarehouseDTO>.Failure(CommonErrors.NotFoundById("warehouse", id));
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
            var errorDetails = validationResult.ToErrorDetails();

            return Result<WarehouseDTO>.Failure(CommonErrors.ValidationError("warehouse", errorDetails));
        }

        warehouseDto.Id = GuidV7.NewGuid();
        var warehouse = WarehouseMapper.ToEntity(warehouseDto);

        await _warehouseRepository.AddAsync(warehouse, cancellationToken);
        await _warehouseRepository.CompleteAsync();

        return Result<WarehouseDTO>.Success(warehouseDto);
    }

    // todo: add comments to the code
    public async Task<Result<WarehouseDTO>> UpdateWarehouseAsync(WarehouseDTO warehouseDto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _warehouseDTOValidator.ValidateAsync(warehouseDto, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            var errorDetails = validationResult.ToErrorDetails();

            return Result<WarehouseDTO>.Failure(CommonErrors.ValidationError("warehouse", errorDetails));
        }

        var warehouse = await _warehouseRepository.FindByIdAsync(warehouseDto.Id, cancellationToken);

        if (warehouse == null)
        {
            return Result<WarehouseDTO>.Failure(CommonErrors.NotFoundById("Warehouse", warehouseDto.Id));
        }

        warehouse.Name = warehouseDto.Name;
        warehouse.Location = warehouseDto.Location;

        _warehouseRepository.Update(warehouse);
        await _warehouseRepository.CompleteAsync();

        return Result<WarehouseDTO>.Success(warehouseDto);
    }

    public async Task<Result<bool>> DeleteWarehouse(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(id, cancellationToken);

        if (warehouse == null)
        {
            return Result<bool>.Failure(CommonErrors.NotFoundById("warehouse", id));
        }

        _warehouseRepository.Delete(warehouse);
        await _warehouseRepository.CompleteAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<byte[]>> GenerateWarehousesReportAsync(CancellationToken cancellationToken = default)
    {
        var warehouses = await _warehouseRepository.GetAllAsync(cancellationToken);
        var warehousesDTO = warehouses.Select(WarehouseMapper.ToDTO);

        var report = _reportService.GenerateReport(warehousesDTO);

        return Result<byte[]>.Success(report);
    }

    public async Task<Result<IEnumerable<WarehouseDTO>>> GetWarehousesWithInventoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var warehouses = await _warehouseRepository.GetWarehousesWithInventoriesAsync(cancellationToken);

        var warehousesDTO = warehouses.Select(WarehouseMapper.ToDTO);

        return Result<IEnumerable<WarehouseDTO>>.Success(warehousesDTO);
    }
}