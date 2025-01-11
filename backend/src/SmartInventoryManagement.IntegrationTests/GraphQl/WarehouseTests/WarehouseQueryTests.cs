using HotChocolate;
using HotChocolate.Execution;
using Snapshooter.Xunit;

namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

public class WarehouseTests : IClassFixture<ServiceSetup>
{
    private readonly ServiceSetup _serviceSetup;

    public WarehouseTests(ServiceSetup serviceSetup)
    {
        _serviceSetup = serviceSetup;
    }

    // [Fact]
    public async Task GetWarehouses_ReturnsPaginatedResults_WithSorting()
    {
        // Arrange & Act
        IExecutionResult result = await _serviceSetup.RequestExecutor.ExecuteAsync(
            @"
                 query{
                   warehouse (take: 5, skip: 0) {
                     items{
                       name
                       location
                     }
                     pageInfo{
                       hasNextPage
                       hasPreviousPage
                     }
                     totalCount
                   }
                 }
                ");

        // Assert
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task GetWarehouseById_ReturnWarehouse_WhenExists()
    {
        // Arrange & Act
        IExecutionResult result = await _serviceSetup.RequestExecutor.ExecuteAsync(
            @"
                query{
                  warehouseById(id: ""089a905d-660d-46d3-97b5-2933747387bc""){
                    id
                    name
                    location
                  }
                }
                ");

        // Assert
        result.ToJson().MatchSnapshot();
    }
    
    [Fact]
    public async Task GetWarehouseById_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var nonExistentId = "00000000-0000-0000-0000-000000000000";

        // Act
        IExecutionResult result = await _serviceSetup.RequestExecutor.ExecuteAsync(
            $@"
            query{{
              warehouseById(id: ""{nonExistentId}""){{
                id
                name
                location
              }}
            }}
        ");
        
        // Assert
        result.ToJson().MatchSnapshot();
    }
    
    // #todo write sorting and projection tests
}
