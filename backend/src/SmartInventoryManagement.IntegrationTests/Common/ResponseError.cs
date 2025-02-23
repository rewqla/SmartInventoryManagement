namespace SmartInventoryManagement.IntegrationTests.Common;

public sealed record ResponseError(
    string Type, 
    string Title, 
    int Status, 
    string Detail, 
    string Instance, 
    List<ErrorDetail> Errors);