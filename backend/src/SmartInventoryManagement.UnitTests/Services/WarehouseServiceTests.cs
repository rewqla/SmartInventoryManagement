using Application.Common;
using Application.DTO.Warehouse;
using Application.Services.Report;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Microsoft.Extensions.Logging;
using Moq;

namespace SmartInventoryManagement.Tests.Services;

public class WarehouseServiceTests
{
    private readonly WarehouseService _warehouseService;
    private readonly Mock<ILogger<WarehouseService>> _logger;
    private readonly WarehouseDTOValidator _warehouseValidator;
    private readonly Mock<IWarehouseRepository> _warehouseRepository;
    private readonly ReportService _reportService;

    public WarehouseServiceTests()
    {
        _warehouseValidator = new WarehouseDTOValidator();
        _warehouseRepository = new Mock<IWarehouseRepository>();
        _logger = new Mock<ILogger<WarehouseService>>();
        _reportService = new ReportService();
        _warehouseService =
            new WarehouseService(_warehouseRepository.Object, _logger.Object, _warehouseValidator, _reportService);
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
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().BeEmpty();
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
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().BeEquivalentTo(expectedWarehouses, options => options
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
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().BeEquivalentTo(expectedWarehouse, options => options
            .Excluding(warehouse => warehouse.Inventories));
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ShouldReturnFailureWithNotFoundError_WhenWarehouseNotFound()
    {
        // Arrange
        var warehouseId = Guid.NewGuid();

        // Act
        var result = await _warehouseService.GetWarehouseByIdAsync(warehouseId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Warehouse.NotFound");
        result.Error.Description.Should().Be($"The warehouse with Id '{warehouseId}' was not found");
        result.Value.Should().BeNull();
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
        var result = await _warehouseService.GetWarehouseByIdAsync(warehouseId);

        // Assert
        result.Error.Code.Should().Be("Warehouse.NotFound");
        result.Error.Description.Should().Be($"The warehouse with Id '{warehouseId}' was not found");

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
        var expectedResult = new Warehouse()
        {
            Name = "Test Warehouse",
            Location = "Test Location"
        };

        _warehouseRepository.Setup(r => r.AddAsync(It.IsAny<Warehouse>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult)
            .Verifiable();

        var warehouseDTO = new WarehouseDTO()
        {
            Name = "Test Warehouse",
            Location = "Test Location"
        };

        // Act
        var result = await _warehouseService.CreateWarehouseAsync(warehouseDTO);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(expectedResult, options => options
            .Excluding(warehouse => warehouse.Id)
            .Excluding(warehouse => warehouse.Inventories));

        _warehouseRepository.Verify(r => r.AddAsync(It.IsAny<Warehouse>(), It.IsAny<CancellationToken>()), Times.Once);
        _warehouseRepository.Verify(r => r.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateWarehouseAsync_ShouldReturnValidationException_WhenObjectIsNotValid()
    {
        // Arrange
        var warehouseDTO = new WarehouseDTO()
        {
            Name = "",
            Location = "Te"
        };

        // Act
        var result = await _warehouseService.CreateWarehouseAsync(warehouseDTO);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Warehouse.ValidationError");
        result.Error.Description.Should().Contain("Some validation problem occured");

        var errorDetails = result.Error.Errors;
        errorDetails.Should().Contain(x => x.PropertyName == "Name" && x.ErrorMessage == "Name is required");
        errorDetails.Should().Contain(x =>
            x.PropertyName == "Location" && x.ErrorMessage == "Location must be at least 3 characters long");
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ShouldUpdateWarehouse_WhenObjectIsValid()
    {
        // Arrange
        var warehouseId = Guid.NewGuid();
        var expectedResult = new Warehouse()
        {
            Id = warehouseId,
            Name = "Test Warehouse",
            Location = "Test Location"
        };

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult)
            .Verifiable();

        _warehouseRepository.Setup(r => r.Update(It.IsAny<Warehouse>()))
            .Returns(expectedResult)
            .Verifiable();

        var warehouseDTO = new WarehouseDTO()
        {
            Id = warehouseId,
            Name = "Test Warehouse",
            Location = "Test Location"
        };

        // Act
        var result = await _warehouseService.UpdateWarehouseAsync(warehouseDTO);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(expectedResult, options => options
            .Excluding(warehouse => warehouse.Id)
            .Excluding(warehouse => warehouse.Inventories));

        _warehouseRepository.Verify(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _warehouseRepository.Verify(r => r.Update(It.IsAny<Warehouse>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ShouldReturnInvalidGuidError_WhenObjectIsNotFound()
    {
        // Arrange
        var warehouseId = Guid.NewGuid();

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Warehouse)null)
            .Verifiable();

        var warehouseDTO = new WarehouseDTO()
        {
            Id = warehouseId,
            Name = "Test Warehouse",
            Location = "Test Location"
        };
        // Act
        var result = await _warehouseService.UpdateWarehouseAsync(warehouseDTO);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Warehouse.NotFound");
        result.Error.Description.Should().Be($"The warehouse with Id '{warehouseId}' was not found");

        _warehouseRepository.Verify(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _warehouseRepository.Verify(r => r.Update(It.IsAny<Warehouse>()), Times.Never);
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ShouldReturnValidationException_WhenObjectIsNotValid()
    {
        // Arrange
        var warehouseDTO = new WarehouseDTO()
        {
            Name = "",
            Location = "Te"
        };

        // Act
        var result = await _warehouseService.UpdateWarehouseAsync(warehouseDTO);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Warehouse.ValidationError");
        result.Error.Description.Should().Contain("Some validation problem occured");

        var errorDetails = result.Error.Errors;
        errorDetails.Should().Contain(x => x.PropertyName == "Name" && x.ErrorMessage == "Name is required");
        errorDetails.Should().Contain(x =>
            x.PropertyName == "Location" && x.ErrorMessage == "Location must be at least 3 characters long");
    }

    [Fact]
    public async Task DeleteWarehouse_ShouldDeleteWarehouse_WhenObjectIsFound()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();
        var warehouse = new Warehouse { Id = warehouseId, Name = "Test Warehouse", Location = "Test Location" };

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouse)
            .Verifiable();

        _warehouseRepository.Setup(r => r.Delete(It.IsAny<Warehouse>()));

        // Act
        var result = await _warehouseService.DeleteWarehouse(warehouseId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        _warehouseRepository.Verify(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _warehouseRepository.Verify(r => r.Delete(It.IsAny<Warehouse>()), Times.Once);
    }

    [Fact]
    public async Task DeleteWarehouse_ShouldDeleteWarehouse_WhenObjectIsNotFound()
    {
        // Arrange
        Guid warehouseId = Guid.NewGuid();

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Warehouse)null)
            .Verifiable();

        // Act
        var result = await _warehouseService.DeleteWarehouse(warehouseId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Warehouse.NotFound");
        result.Error.Description.Should().Be($"The warehouse with Id '{warehouseId}' was not found");
        _warehouseRepository.Verify(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        _warehouseRepository.Verify(r => r.Delete(It.IsAny<Warehouse>()), Times.Never);
    }

    // #todo: write tests with inventories
    // #todo: write tests with CancellationToken
}