using API;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using API.Endpoints; 
using Testcontainers.PostgreSql;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests;

public class CustomApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15")
        .Build();

    public string ConnectionString => _postgresqlContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(InventoryContext));

            services.AddDbContext<InventoryContext>(options =>
                options.UseNpgsql(ConnectionString));
        });
    }

    private async Task SeedDataAsync(InventoryContext dbContext)
    {
        if (!dbContext.Warehouses.Any())
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc"), Name = "Warehouse A", Location = "New York"
                },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse B", Location = "Rivne" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse C", Location = "Berlin" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse D", Location = "Tokyo" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse E", Location = "London" },
            };

            await dbContext.Warehouses.AddRangeAsync(warehouses);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task InitializeAsync()
    {
        await _postgresqlContainer.StartAsync();

        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();

        await dbContext.Database.MigrateAsync();

        await SeedDataAsync(dbContext);
    }

    public async Task DisposeAsync()
    {
        await _postgresqlContainer.StopAsync();
    }
}