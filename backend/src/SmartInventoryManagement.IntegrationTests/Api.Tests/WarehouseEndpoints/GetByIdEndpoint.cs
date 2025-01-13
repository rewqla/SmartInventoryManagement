using System.Net;
using System.Net.Http.Json;
using API;
using Application.Common;
using Application.DTO.Warehouse;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.WarehouseEndpoints;

public class GetByIdEndpoint : 
    IClassFixture<WebApplicationFactory<IApiMarker>>

{
    private readonly HttpClient _httpClient;

    public GetByIdEndpoint(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }
    
    [Fact]
    public async Task GetById_ReturnsWarehouse_WhenWarehouseExists()
    {
        // Arrange
        var validId = "ec4d732c-45ba-4478-bc9f-a2ea2519483f";
        
        // Act
        var response = await _httpClient
            .GetAsync($"/api/warehouses/{validId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var warehouse = await response.Content.ReadFromJsonAsync<WarehouseDTO>();
        
        warehouse.Should().NotBeNull();
        warehouse!.Id.Should().Be(Guid.Parse(validId));
        warehouse.Name.Should().NotBeNullOrEmpty();
        warehouse.Location.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WheWarehouseNotFound()
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
    
    // todo: add class with warehouseDTO object for further use
}