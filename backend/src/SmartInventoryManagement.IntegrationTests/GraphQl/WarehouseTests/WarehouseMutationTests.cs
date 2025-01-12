using HotChocolate;
using HotChocolate.Execution;
using Snapshooter.Xunit;

namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

public class WarehouseMutationTests: IClassFixture<GraphQLServiceSetup>
{
    private readonly GraphQLServiceSetup _graphQlServiceSetup;

    public WarehouseMutationTests(GraphQLServiceSetup graphQlServiceSetup)
    {
        _graphQlServiceSetup = graphQlServiceSetup;
    }

    [Fact]
    public async Task GetWarehouses_FilterByLocation_ReturnsMatchingResults()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                mutation{
                  createWarehouse(input:  {
                         name: ""Normal Warehouse"",
                         location: ""Lutsk""
                      }){
                      name
                      location
                      __typename
                      errors{
                        ...on ValidationError{
                          message
                          errors{
                            propertyName
                            errorMessage
                          }
                        }
                      }
                    }
                  }");

        // Assert
        result.ToJson().MatchSnapshot();
    }
}
