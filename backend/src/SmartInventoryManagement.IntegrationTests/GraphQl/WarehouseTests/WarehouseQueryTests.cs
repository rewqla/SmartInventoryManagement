using HotChocolate;
using HotChocolate.Execution;
using Snapshooter.Xunit;

namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

public class WarehouseQueryTests: IClassFixture<GraphQLServiceSetup>
{
  private readonly GraphQLServiceSetup _graphQlServiceSetup;

  public WarehouseQueryTests(GraphQLServiceSetup graphQlServiceSetup)
  {
    _graphQlServiceSetup = graphQlServiceSetup;
  }

    [Fact]
    public async Task GetWarehouses_FilterByLocation_ReturnsMatchingResults()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(where: { location: { eq: ""New York"" } }) {
            items {
              name
              location
            }
            totalCount
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouses_FilterWithOrCondition_ReturnsMatchingResults()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(
            where: {
              or: [
                { name: { contains: ""Warehouse A"" } },
                { location: { eq: ""Rivne"" } }
              ]
            }
          ) {
            items {
              name
              location
            }
            totalCount
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouses_FilterWithAndCondition_ReturnsMatchingResults()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(
            where: {
              or: [
                { name: { contains: ""Warehouse B"" } },
                { location: { eq: ""Rivne"" } }
              ]
            }
          ) {
            items {
              name
              location
            }
            totalCount
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouses_SortedByNameDescending_ReturnsSortedResults()
    {
      // Arrange & Act
      IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
        @"
        query {
          warehouse(order: [{ name: DESC }]) {
            items {
              name
              location
            }
            totalCount
          }
        }");

      // Assert
      result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouses_FilteredAndSorted_ReturnsFilteredAndSortedResults()
    {
      // Arrange & Act
      IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
        @"
        query {
          warehouse(
            where: { location: { eq: ""London"" } },
            order: [{ name: ASC }]
          ) {
            items {
              name
              location
            }
            totalCount
          }
        }");

      // Assert
      result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouses_NoResultsForFilter_ReturnsEmpty()
    {
      // Arrange & Act
      IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
        @"
        query {
          warehouse(where: { location: { eq: ""NonExistentLocation"" } }) {
            items {
              name
              location
            }
            totalCount
          }
        }");

      // Assert
      result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouseById_ReturnWarehouse_WhenExists()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
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
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
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
    
    // #todo write pagination tests
    // #todo write query test with sorting, projection, filtering, pagination
    // #todo projection tests
    
    // #todo write tests for mutation create
    // #todo write tests for mutation delete
    // #todo write tests for mutation update

    // #todo write tests subscription
    
    // #todo add global imports for tests
}
