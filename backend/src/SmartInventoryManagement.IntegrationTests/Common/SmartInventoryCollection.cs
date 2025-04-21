using SmartInventoryManagement.IntegrationTests.Api.Tests;

namespace SmartInventoryManagement.IntegrationTests.Common;

[CollectionDefinition(nameof(SmartInventoryCollection))]
public class SmartInventoryCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}