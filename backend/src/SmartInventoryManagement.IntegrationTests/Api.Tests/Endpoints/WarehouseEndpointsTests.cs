using System.Net;
using System.Net.Http.Json;
using API;
using Application.Common;
using Application.DTO.Warehouse;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartInventoryManagement.IntegrationTests.Helpers;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.Endpoints;

public class WarehouseEndpointsTests : 
    IClassFixture<WebApplicationFactory<IApiMarker>>, IClassFixture<WarehouseTestFixture>
{
    private readonly HttpClient _httpClient;
    private readonly WarehouseTestFixture _testFixture;
    
    public WarehouseEndpointsTests(WebApplicationFactory<IApiMarker> appFactory, WarehouseTestFixture testFixture)
    {
        _testFixture = testFixture;
        _httpClient = appFactory.CreateClient();
    }
    
    [Fact]
    public async Task CreateWarehouse_ReturnsOk_WhenValidWarehouseIsProvided()
    {
        // Arrange
        var warehouseDto = _testFixture.GetWarehouseDTO();

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

        var errorResponse = await response.Content.ReadFromJsonAsync<Error>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Code.Should().Be("Warehouse.ValidationError");
        errorResponse.Description.Should().Contain("Some validation problem occured");
        errorResponse.Errors.Should().NotBeEmpty();
        errorResponse.Errors.Should().Contain(d => d.PropertyName == "Name" && d.ErrorMessage.Contains("Name is required"));
    }

    [Fact]
    public async Task GetById_ReturnsWarehouse_WhenWarehouseExists()
    {
        // Arrange
        var warehouseId = _testFixture.CreatedWarehouseId;
        
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
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenWarehouseNotFound()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var response = await _httpClient
            .GetAsync($"/api/warehouses/{id}");

        // Assert
        var errorResponse = await response.Content.ReadFromJsonAsync<Error>();
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        errorResponse.Should().NotBeNull();
        errorResponse!.Code.Should().Be("Warehouse.NotFound");
        errorResponse!.Description.Should().Be($"The warehouse with Id '{id}' was not found");
    }
}