﻿using HotChocolate;
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
    public async Task CreateWarehouse_ReturnsWarehouse_WhenObjectIsValid()
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
    
    [Fact]
    public async Task CreateWarehouse_ReturnsValidationError_WhenNameIsEmpty()
    {
      // Arrange & Act
      IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
        @"mutation{
                 createWarehouse(input:  {
                   name: """",
                   location: ""Lutsk""
                 }){
                   name
                   location
                   id
                   __typename
                   errors{
                     __typename
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
    
    // todo: add validation error test
    // todo: add not found error test
    [Fact]
    public async Task UpdateWarehouse_ReturnsValidationError_WhenLocationIsEmpty()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"mutation{
                     updateWarehouse(input:  {
                      id: ""b24ab279-1fd3-4fb0-9107-8563612aee1f"",
                      name: ""Secret Warehouse"",
                       location: """"
                     }){
                        name
                        location
                        id
                        errors {
                         __typename
                         ...on ValidationError{
                           message
                           errors{
                             propertyName
                             errorMessage
                           }
                         }
                         ... on EntityNotFoundError {
                           message
                         }
                       }
                     }
                   }");

        // Assert
        result.ToJson().MatchSnapshot();
    }
    
    [Fact]
    public async Task UpdateWarehouse_ReturnsUpdatedWarehouse_WhenInputIsValid()
    {
      // Arrange & Act
      IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
        @"
               mutation{
                   updateWarehouse(input:  {
                      id: ""b24ab279-1fd3-4fb0-9107-8563612aee1f"",
                      name: ""Secret Warehouse"",
                      location: ""Vinnytsia""
                   }){
                    id
                    name
                    location
                    errors {
                     __typename
                     ... on EntityNotFoundError {
                       message
                     }
                   }
                 }
               }");

      // Assert
      result.ToJson().MatchSnapshot();
    }
    
    // todo: add not found warehouse test
    // todo: execute only after update
    [Fact]
    public async Task DeleteWarehouse_ReturnsTrue_WhenGuidIsFound()
    {
        // Arrange & Act
        IExecutionResult result = await _graphQlServiceSetup.RequestExecutor.ExecuteAsync(
            @"
              mutation{
                deleteWarehouse(input:  {
                   warehouseId: ""b24ab279-1fd3-4fb0-9107-8563612aee1f""
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

        // Assert
        result.ToJson().MatchSnapshot();
    }
}
