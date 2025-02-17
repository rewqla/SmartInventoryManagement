namespace SmartInventoryManagement.IntegrationTests.GraphQl.WarehouseTests;

public class WarehouseSubscriptionTests : IClassFixture<GraphQLServiceSetup>
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

        await foreach (var item in result.ExpectResponseStream()
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

    [Fact]
    public async Task WarehouseUpdated_SubscriptionReceivesEvent()
    {
        // Arrange & Act
        using var cts = new CancellationTokenSource(1_000);
        await using var result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                subscription{
                  warehouseUpdated(warehouseId: ""4e9ae812-9308-41e7-aaa5-32379c4c2b3d""){
                    name
                    location
                    id
                  }
                }");

        await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                mutation{
                    updateWarehouse(input:  {
                       id: ""4e9ae812-9308-41e7-aaa5-32379c4c2b3d"",
                       name: ""Secret Warehouse"",
                       location: ""Vinnytsia""
                    }){
                     name
                     location
                     id
                     errors {
                      __typename
                      ... on EntityNotFoundError {
                        message
                      }
                    }
                  }
                }");

        var items = new List<object>();
        var count = 0;

        await foreach (var item in result.ExpectResponseStream()
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
    
    [Fact]
    public async Task WarehouseMutated_SubscriptionReceivesEvent_OnWarehouseCreate()
    {
        // Arrange & Act
        using var cts = new CancellationTokenSource(1_000);
        await using var result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                subscription{
                  warehouseMutated{
                    eventType
                    warehouse{
                      name
                      location
                    }
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

        await foreach (var item in result.ExpectResponseStream()
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
    
    [Fact]
    public async Task WarehouseMutated_SubscriptionReceivesEvent_OnWarehouseUpdate()
    {
        // Arrange & Act
        using var cts = new CancellationTokenSource(1_000);
        await using var result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                subscription{
                  warehouseMutated{
                    eventType
                    warehouse{
                      name
                      location
                    }
                  }
                }");

        await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                 mutation{
                    updateWarehouse(input:  {
                       id: ""862038bf-9363-4552-92e8-2f05e9b2fe9e"",
                       name: ""Secret Warehouse"",
                       location: ""Vinnytsia""
                    }){
                     name
                     location
                     id
                     errors {
                      __typename
                      ... on EntityNotFoundError {
                        message
                      }
                    }
                  }
                }");

        var items = new List<object>();
        var count = 0;

        await foreach (var item in result.ExpectResponseStream()
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
    
    [Fact]
    public async Task WarehouseMutated_SubscriptionReceivesEvent_OnWarehouseDelete()
    {
        // Arrange & Act
        using var cts = new CancellationTokenSource(1_000);
        await using var result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                subscription{
                  warehouseMutated{
                    eventType
                    warehouse{
                      name
                      location
                    }
                  }
                }");

        await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
                 mutation{
                   deleteWarehouse(input:  {
                      warehouseId: ""697f1876-2cbd-4c1e-a075-7554fe1977bc""
                   }){
                     boolean
                     errors {
                       __typename
                       ... on EntityNotFoundError {
                         message
                       }
                     }
                   }
                 }");

        var items = new List<object>();
        var count = 0;

        await foreach (var item in result.ExpectResponseStream()
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