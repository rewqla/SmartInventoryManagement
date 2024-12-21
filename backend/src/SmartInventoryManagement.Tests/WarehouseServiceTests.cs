using Application.Errors;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
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
    private readonly Mock<WarehouseDTOValidator> _warehouseValidator;
    private readonly Mock<IWarehouseRepository> _warehouseRepository;

    public WarehouseServiceTests()
    {
        _warehouseValidator = new Mock<WarehouseDTOValidator>();
        _warehouseRepository = new Mock<IWarehouseRepository>();
        _logger = new Mock<ILogger<WarehouseService>>();
        _warehouseService =
            new WarehouseService(_warehouseRepository.Object, _logger.Object, _warehouseValidator.Object);
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
                It.Is<It.IsAnyType>((o, t) => string.Equals("Retrieve all warehouses", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ShouldReturnWarehouse_WhenWarehouseExistsWithoutInventories()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();
        var expectedWarehouse = new Warehouse
        {
            Id = warehouseId,
            Name = "Main Warehouse",
            Location = "Rivne"
        };

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedWarehouse)
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehouseByIdAsync(warehouseId);

        // Assert
        result.Should().BeEquivalentTo(expectedWarehouse, options => options
            .Excluding(warehouse => warehouse.Inventories));
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ShouldReturnException_WhenWarehouseNotFound()
    {
        // Arrange
        var warehouseId = Guid.NewGuid();

        // Act
        var action = async () => await _warehouseService.GetWarehouseByIdAsync(warehouseId);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidGuidError>(action);
        Assert.Equal($"Warehouse {warehouseId} not found", exception.Message);
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();
        var expectedWarehouse = new Warehouse
        {
            Id = warehouseId,
            Name = "Main Warehouse",
            Location = "Rivne"
        };

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedWarehouse)
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehouseByIdAsync(warehouseId);

        // Assert
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals($"Retrieve warehouse with id: {warehouseId}", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals($"Warehouse with id {warehouseId} retrieved", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ShouldLogMessages_WhenWarehouseNotFound()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();

        // Act
        var action = async () => await _warehouseService.GetWarehouseByIdAsync(warehouseId);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidGuidError>(action);
        Assert.Equal($"Warehouse {warehouseId} not found", exception.Message);

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals($"Warehouse with id {warehouseId} not found", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateWarehouseAsync_ShouldCreateUser_WhenObjectIsValid()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task CreateWarehouseAsync_ShouldReturnValidationException_WhenObjectIsNotValid()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ShouldUpdateWarehouse_WhenObjectIsValid()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ShouldReturnInvalidGuidError_WhenObjectIsNotFound()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ShouldReturnValidationException_WhenObjectIsNotValid()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task DeleteWarehouse_ShouldDeleteWarehouse_WhenObjectIsFound()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task DeleteWarehouse_ShouldDeleteWarehouse_WhenObjectIsNotFound()
    {
        // Arrange

        // Act

        // Assert
    }
}