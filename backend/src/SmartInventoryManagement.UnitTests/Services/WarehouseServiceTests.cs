
namespace SmartInventoryManagement.Tests.Services;

public class WarehouseServiceTests
{
    private readonly WarehouseService _warehouseService;
    private readonly Mock<ILogger<WarehouseService>> _logger;
    private readonly WarehouseDTOValidator _warehouseValidator;
    private readonly Mock<IWarehouseRepository> _warehouseRepository;
    private readonly Mock<IReportService> _reportService;

    public WarehouseServiceTests()
    {
        _warehouseValidator = new WarehouseDTOValidator();
        _warehouseRepository = new Mock<IWarehouseRepository>();
        _logger = new Mock<ILogger<WarehouseService>>();
        _reportService = new Mock<IReportService>();
        _warehouseService =
            new WarehouseService(_warehouseRepository.Object, _logger.Object, _warehouseValidator,
                _reportService.Object);
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
        var warehouses = new WarehouseFaker().Generate(2);

        _warehouseRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouses)
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehousesAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().BeEquivalentTo(warehouses, options => options
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
        var expectedWarehouse = new WarehouseFaker().Generate();
        var warehouseId = expectedWarehouse.Id;

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
        var warehouseId = GuidV7.NewGuid();

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
        var expectedWarehouse = new WarehouseFaker().Generate();
        var warehouseId = expectedWarehouse.Id;

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
        Guid warehouseId = GuidV7.NewGuid();

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
    public async Task CreateWarehouseAsync_ShouldCreateWarehouse_WhenObjectIsValid()
    {
        // Arrange
        var expectedResult = new WarehouseFaker().Generate();

        _warehouseRepository.Setup(r => r.AddAsync(It.IsAny<Warehouse>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult)
            .Verifiable();

        var warehouseDTO = new WarehouseDTO()
        {
            Name = expectedResult.Name,
            Location = expectedResult.Location
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
        var expectedResult = new WarehouseFaker().Generate();
        var warehouseId = expectedResult.Id;

        _warehouseRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult)
            .Verifiable();

        _warehouseRepository.Setup(r => r.Update(It.IsAny<Warehouse>()))
            .Returns(expectedResult)
            .Verifiable();

        var warehouseDTO = new WarehouseDTO()
        {
            Id = warehouseId,
            Name = expectedResult.Name,
            Location = expectedResult.Location
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
        var warehouseId = GuidV7.NewGuid();

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
        var warehouse = new WarehouseFaker().Generate();
        var warehouseId = warehouse.Id;
        
        
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
        Guid warehouseId = GuidV7.NewGuid();

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

    [Fact]
    public async Task GenerateWarehousesReportAsync_ShouldReturnReport_WhenWarehousesExist()
    {
        // Arrange
        var warehouses = new WarehouseFaker().Generate(5);

        _warehouseRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouses);

        var reportBytes = new byte[] { 1, 2, 3, 4, 5 };
        _reportService.Setup(r => r.GenerateReport(It.IsAny<IEnumerable<WarehouseDTO>>()))
            .Returns(reportBytes);

        // Act
        var result = await _warehouseService.GenerateWarehousesReportAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(reportBytes);

        _reportService.Verify(r => r.GenerateReport(It.IsAny<IEnumerable<WarehouseDTO>>()), Times.Once);
    }

    [Fact]
    public async Task GenerateWarehousesReportAsync_ShouldReturnReport_WhenNoWarehouses()
    {
        // Arrange
        var warehouses = new List<Warehouse>();  

        _warehouseRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouses);

        var reportBytes = new byte[] { 1, 2, 3, 4, 5 };  // Default report (non-empty)
        _reportService.Setup(r => r.GenerateReport(It.IsAny<IEnumerable<WarehouseDTO>>()))
            .Returns(reportBytes);

        // Act
        var result = await _warehouseService.GenerateWarehousesReportAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(reportBytes);  

        _warehouseRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _reportService.Verify(r => r.GenerateReport(It.IsAny<IEnumerable<WarehouseDTO>>()), Times.Once);
    }

    [Fact]
    public async Task GetWarehousesWithInventoriesAsync_ShouldReturnEmpty_WhenNoWarehouses()
    {
        // Arrange
        _warehouseRepository.Setup(r => r.GetWarehousesWithInventoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Warehouse>())
            .Verifiable();

        // Act
        var result = await _warehouseService.GetWarehousesWithInventoriesAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Value.Should().BeEmpty();
    }
}