using System.Net;
using System.Net.Http.Json;
using API;
using Application.Common;
using Application.DTO.Warehouse;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartInventoryManagement.IntegrationTests.Helpers;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.WarehouseEndpoints;

public class CreateEndpoint: 
    IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;
    private Guid _createdWarehouseId;
    public CreateEndpoint(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }
    
    [Fact]
    public async Task CreateWarehouse_ReturnsOk_WhenValidWarehouseIsProvided()
    {
        // Arrange
        var warehouseDto = WarehouseTestHelper.GetWarehouseDTO();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/warehouses", warehouseDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdWarehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();
        createdWarehouse.Should().NotBeNull();
        createdWarehouse!.Id.Should().NotBe(Guid.Empty);
        createdWarehouse.Name.Should().Be(warehouseDto.Name);
        createdWarehouse.Location.Should().Be(warehouseDto.Location);
        
        WarehouseTestHelper.CreatedWarehouseId = createdWarehouse.Id;
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
}