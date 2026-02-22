using SmartInventoryManagement.IntegrationTests.Common;
using SmartInventoryManagement.IntegrationTests.Helpers;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.WarehouseEnpoints;

// Sends http requests to the TestContainer by CustomWebApplicationFactory
[Collection(nameof(SmartInventoryCollection))]
public class WarehouseEndpointsTestsCustom
{
    private readonly HttpClient _httpClient;

    public WarehouseEndpointsTestsCustom(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task CreateWarehouse_ReturnsOk_WhenValidWarehouseIsProvided()
    {
        // Arrange
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO();
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/warehouses", warehouseDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdWarehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();
        createdWarehouse.Should().NotBeNull();
        createdWarehouse!.Id.Should().NotBe(Guid.Empty);
        createdWarehouse.Name.Should().Be(warehouseDto.Name);
        createdWarehouse.Location.Should().Be(warehouseDto.Location);
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
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));


        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/warehouses", invalidWarehouseDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse.Title.Should().Be("Warehouse.ValidationError");
        errorResponse.Detail.Should().Contain("Some validation problem occured");
        errorResponse.Errors.Should().NotBeEmpty();
        errorResponse.Errors.Should()
            .Contain(d => d.PropertyName == "name" && d.ErrorMessage.Contains("Name is required"));
    }

    [Fact]
    public async Task GetById_ReturnsWarehouse_WhenWarehouseExists()
    {
        // Arrange
        var warehouseId = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc");
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

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenWarehouseNotFound()
    {
        // Arrange
        var id = Guid.Empty;
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

        // Act
        var response = await _httpClient
            .GetAsync($"/api/warehouses/{id}");

        // Assert
        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.NotFound");
        errorResponse!.Detail.Should().Be($"The warehouse with Id '{id}' was not found");

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task UpdateWarehouse_ReturnsOk_WhenValidWarehouseIsProvided()
    {
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO("Updated Warehouse", "Updated Location");
        warehouseDto.Id = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc");
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

        var response = await _httpClient.PutAsJsonAsync($"/api/warehouses/{warehouseDto.Id}", warehouseDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedWarehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();
        updatedWarehouse.Should().NotBeNull();
        updatedWarehouse!.Id.Should().Be(warehouseDto.Id);
        updatedWarehouse.Name.Should().Be(warehouseDto.Name);
        updatedWarehouse.Location.Should().Be(warehouseDto.Location);
    }

    [Fact]
    public async Task UpdateWarehouse_ReturnsNotFound_WhenWarehouseDoesNotExist()
    {
        var nonExistentId = GuidV7.NewGuid();
        var warehouseDto = WarehouseTestFixture.GetWarehouseDTO();
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

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
            Id = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc"),
            Name = "",
            Location = "Updated Location"
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

        var response = await _httpClient.PutAsJsonAsync($"/api/warehouses/{warehouseDto.Id}", warehouseDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Warehouse.ValidationError");
        errorResponse.Detail.Should().Contain("Some validation problem occured");
        errorResponse.Errors.Should().NotBeEmpty();
        errorResponse.Errors.Should()
            .Contain(d => d.PropertyName == "name" && d.ErrorMessage.Contains("Name is required"));
    }

    [Fact]
    public async Task DeleteWarehouse_ReturnsNotFound_WhenWarehouseDoesNotExist()
    {
        // Arrange
        var nonExistentId = GuidV7.NewGuid();
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

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
        var warehouseId = Guid.Parse("b24ab279-1fd3-4fb0-9107-8563612aee1f");
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

        // Act
        var response = await _httpClient.DeleteAsync($"/api/warehouses/{warehouseId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
