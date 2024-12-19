﻿using Application.Services.Warehouse;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Moq;

namespace SmartInventoryManagement.Tests;

public class WarehouseServiceTests
{
    private readonly WarehouseService _warehouseService;
    private readonly Mock<IWarehouseRepository> _warehouseRepository;

    public WarehouseServiceTests()
    {
        _warehouseRepository = new Mock<IWarehouseRepository>();
        _warehouseService = new WarehouseService(_warehouseRepository.Object);
    }

    [Fact]
    public async Task GetWarehousesAsync_ShouldReturnEmpty_WhenNoWarehouses()
    {
        // Arrange
        _warehouseRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Warehouse>())
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehousesAsync();
        
        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWarehousesAsync_ShouldReturnEnumerableWarehouses_WhenWarehousesExist()
    {

    }
}