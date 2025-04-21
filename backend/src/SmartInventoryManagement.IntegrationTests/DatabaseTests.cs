using SmartInventoryManagement.IntegrationTests.Api.Tests;
using SmartInventoryManagement.IntegrationTests.Common;

namespace SmartInventoryManagement.IntegrationTests;

[Collection(nameof(SmartInventoryCollection))]
public class DatabaseTests : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _applicationFactory;

    public DatabaseTests(CustomWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task DatabaseTest_ShouldAllMigrationBeInstalled()
    {
        // Arrange
        using var scope = _applicationFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<InventoryContext>();

        var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
        var availableMigrations = context.Database.GetMigrations();

        // Assert
        appliedMigrations.Should().BeEquivalentTo(availableMigrations);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}

//todo: refactor CustomWebApplicationFactory
