using SmartInventoryManagement.IntegrationTests.Common;
using SmartInventoryManagement.IntegrationTests.Helpers;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.Endpoints;

public class WarehouseEndpointsTests :
    IClassFixture<WebApplicationFactory<ISmartInventoryHost>>, IClassFixture<WarehouseTestFixture>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WarehouseTestFixture _testFixture;

    public WarehouseEndpointsTests(WebApplicationFactory<ISmartInventoryHost> appFactory,
        WarehouseTestFixture testFixture)
    {
        _testFixture = testFixture;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task CreateWarehouse_ReturnsOk_WhenValidWarehouseIsProvided()
    {
        // Arrange
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/warehouses", warehouseDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdWarehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();
        createdWarehouse.Should().NotBeNull();
        createdWarehouse!.Id.Should().NotBe(Guid.Empty);
        createdWarehouse.Name.Should().Be(warehouseDto.Name);
        createdWarehouse.Location.Should().Be(warehouseDto.Location);

        _testFixture.CreatedWarehouseId = createdWarehouse.Id;
    }

    [Fact]
    public async Task CreateWarehouse_ReturnsBadRequest_WhenServiceFails()
    {
        // Arrange
        var invalidWarehouseDto = new WarehouseDTO
        {
            Name = "",
            Location = "Test Location"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/warehouses", invalidWarehouseDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.ValidationError");
        errorResponse.Detail.Should().Contain("Some validation problem occured");
        errorResponse.Errors.Should().NotBeEmpty();
        errorResponse.Errors.Should()
            .Contain(d => d.PropertyName == "Name" && d.ErrorMessage.Contains("Name is required"));
    }

    [Fact]
    public async Task GetById_ReturnsWarehouse_WhenWarehouseExists()
    {
        // Arrange
        var warehouseId = _testFixture.CreatedWarehouseId;
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));
        
        // Act
        var response = await _httpClient
            .GetAsync($"/api/warehouses/{warehouseId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var warehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();

        warehouse.Should().NotBeNull();
        warehouse!.Id.Should().Be(warehouseId);
        warehouse.Name.Should().NotBeNullOrEmpty();
        warehouse.Location.Should().NotBeNullOrEmpty();
        
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    //todo: update test for get by id to overcome unauthorized and send requests with token
    [Fact]
    public async Task GetById_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/warehouses/{id}");

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenWarehouseNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));
        // Act
        var response = await _httpClient
            .GetAsync($"/api/warehouses/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.NotFound");
        errorResponse!.Detail.Should().Be($"The warehouse with Id '{id}' was not found");
        
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task UpdateWarehouse_ReturnsOk_WhenValidWarehouseIsProvided()
    {      
        // Arrange
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO("Updated Warehouse", "Updated Location");
        warehouseDto.Id = _testFixture.CreatedWarehouseId;
        
        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/warehouses/{warehouseDto.Id}", warehouseDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedWarehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();
        updatedWarehouse.Should().NotBeNull();
        updatedWarehouse!.Id.Should().Be(warehouseDto.Id);
        updatedWarehouse.Name.Should().Be(warehouseDto.Name);
        updatedWarehouse.Location.Should().Be(warehouseDto.Location);

        _testFixture.isUpdated = true;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task UpdateWarehouse_ReturnsNotFound_WhenWarehouseDoesNotExist()
    {
        var nonExistentId = GuidV7.NewGuid();
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO();

        var response = await _httpClient.PutAsJsonAsync($"/api/warehouses/{nonExistentId}", warehouseDto);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.NotFound");
        errorResponse.Detail.Should().Be($"The warehouse with Id '{nonExistentId}' was not found");
    }

    [Fact]
    public async Task UpdateWarehouse_ReturnsBadRequest_WhenValidationFails()
    {
        var warehouseDto = new WarehouseDTO
        {
            Id = _testFixture.CreatedWarehouseId,
            Name = "",
            Location = "Updated Location"
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/warehouses/{warehouseDto.Id}", warehouseDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.ValidationError");
        errorResponse.Detail.Should().Contain("Some validation problem occured");
        errorResponse.Errors.Should().NotBeEmpty();
        errorResponse.Errors.Should()
            .Contain(d => d.PropertyName == "Name" && d.ErrorMessage.Contains("Name is required"));
    }

    [Fact]
    public async Task DeleteWarehouse_ReturnsNotFound_WhenWarehouseDoesNotExist()
    {
        // Arrange
        var nonExistentId = GuidV7.NewGuid();

        // Act
        var response = await _httpClient.DeleteAsync($"/api/warehouses/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.NotFound");
        errorResponse.Detail.Should().Be($"The warehouse with Id '{nonExistentId}' was not found");
    }

    [Fact]
    public async Task DeleteWarehouse_ReturnsOk_WhenWarehouseExists()
    {
        // Arrange
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO();

        // Act
        var createResponse = await _httpClient.PostAsJsonAsync("/api/warehouses", warehouseDto);

        // Assert
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Arrange

        var createdWarehouse = await createResponse.Content.ReadFromJsonAsync<WarehouseDTO>();
        var warehouseId = createdWarehouse?.Id;

        // Act
        var response = await _httpClient.DeleteAsync($"/api/warehouses/{warehouseId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public async void Dispose()
    {
        if (_testFixture.isUpdated == false)
            await Task.Delay(1000);

        await _httpClient.DeleteAsync($"/api/warehouses/{_testFixture.CreatedWarehouseId}");
    }
}