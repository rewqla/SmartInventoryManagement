using HotChocolate;
using HotChocolate.Execution;
using Snapshooter.Xunit;

namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

public class WarehouseMutationTests : IClassFixture<ServiceSetup>
{
    private readonly ServiceSetup _serviceSetup;

    public WarehouseMutationTests(ServiceSetup serviceSetup)
    {
        _serviceSetup = serviceSetup;
    }

    [Fact]
    public async Task GetWarehouses_FilterByLocation_ReturnsMatchingResults()
    {
        // Arrange & Act
        IExecutionResult result = await _serviceSetup.RequestExecutor.ExecuteAsync(
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
