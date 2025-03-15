namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

[Collection("GraphQLServiceCollection")]
public class WarehouseQueryTests
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
                { name: { contains: ""Warehouse K"" } },
                { location: { eq: ""Amsterdam"" } }
              ]
            }
            order: [ {
               name: ASC
            }]
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
              and: [
                { name: { contains: ""Warehouse K"" } },
                { location: { eq: ""Sydney"" } }
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

    [Fact]
    public async Task GetWarehouses_TakeForty_ReturnsExceedError()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 40) {
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
    public async Task GetWarehouses_TotalCount_ReturnsCorrectValue()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 20) {
            totalCount
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task GetWarehouses_TakeZero_ReturnsEmpty()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 0) {
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
    public async Task GetWarehouses_TakeTen_ReturnsRelevantCount()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 10,
            order: [{ name: ASC }]) {
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
    public async Task GetWarehouses_Pagination_ReturnsPageInfoCorrectly()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 5,
            order: [{ name: ASC }]) {
            items {
              name
              location
            }
            pageInfo {
              hasNextPage
              hasPreviousPage
            }
            totalCount
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task GetWarehouses_SkipFive_ReturnsCorrectSubset()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 10, skip: 5,
            order: [{ name: ASC }]) {
            items {
              name
              location
            }
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task GetWarehouses_Pagination_TakeSkip_TotalCount_PageInfo_ReturnsCorrectResponse()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
        query {
          warehouse(take: 10, skip: 5,
            order: [{ name: ASC }]) {
            items {
              name
              location
            }
            pageInfo {
              hasNextPage
              hasPreviousPage
            }
            totalCount
          }
        }");

        // Assert
        result.ToJson().MatchSnapshot();
    }
    // #todo write projection tests
}