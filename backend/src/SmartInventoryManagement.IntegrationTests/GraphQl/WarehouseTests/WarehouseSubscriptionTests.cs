namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

public class WarehouseSubscriptionTests: IClassFixture<GraphQLServiceSetup>
{
    private readonly GraphQLServiceSetup _graphQlServiceSetup;

    public WarehouseSubscriptionTests(GraphQLServiceSetup graphQlServiceSetup)
    {
        _graphQlServiceSetup = graphQlServiceSetup;
    }

    
    [Fact]
    public async Task WarehouseCreated_SubscriptionReceivesEvent()
    {
        // Arrange & Act
        
        using var cts = new CancellationTokenSource(1_000);
        await using var result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                subscription{
                  warehouseCreated{
                    name
                    location
                  }
                }");
        
        await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                mutation{
                  createWarehouse(input:  {
                         name: ""Normal Warehouse"",
                         location: ""Lutskiy""
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
        
        var items = new List<object>();
        var count = 0;

        await foreach(var item in result.ExpectResponseStream()
                          .ReadResultsAsync().WithCancellation(cts.Token))
        {
            items.Add(item.Data);
            count++;
            if (count == 2)
            {
                break;
            }
        }
        
        // Assert
        items.MatchSnapshot();
    }
}