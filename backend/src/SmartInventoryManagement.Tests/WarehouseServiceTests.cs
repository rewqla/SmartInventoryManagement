using Application.Services.Warehouse;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Microsoft.Extensions.Logging;
using Moq;

namespace SmartInventoryManagement.Tests;

public class WarehouseServiceTests
{
    private readonly WarehouseService _warehouseService;
    private readonly Mock<ILogger<WarehouseService>> _logger;
    private readonly Mock<IWarehouseRepository> _warehouseRepository;

    public WarehouseServiceTests()
    {
        _warehouseRepository = new Mock<IWarehouseRepository>();
        _logger = new Mock<ILogger<WarehouseService>>();
        _warehouseService = new WarehouseService(_warehouseRepository.Object, _logger.Object);
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
    public async Task GetWarehousesAsync_ShouldReturnEnumerableWarehouses_WhenWarehousesExistWithoutInventories()
    {
        // Arrange
        var rivneWarehouse = new Warehouse()
        {
            Id = Guid.NewGuid(),
            Name = "Main Warehouse",
            Location = "Rivne "
        };

        var expectedWarehouses = new[] { rivneWarehouse };

        _warehouseRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedWarehouses)
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehousesAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedWarehouses, options => options
            .Excluding(warehouse => warehouse.Inventories));
    }
    
    [Fact]
    public async Task GetWarehousesAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        _warehouseRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Warehouse>())
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehousesAsync();

        // Assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals("Retrieve all warehouses", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}