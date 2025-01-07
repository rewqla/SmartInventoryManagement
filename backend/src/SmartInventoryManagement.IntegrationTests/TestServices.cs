using API.GraphQL.Queries;
using Application.Interfaces.Services.Warehouse;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
using HotChocolate.Execution;
using Infrastructure.Data;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Infrastructure.Repositories.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Snapshooter;

namespace SmartInventoryManagement.IntegrationTests;

public class TestServices: IAsyncLifetime
{
    private IRequestExecutor _requestExecutor = null!;
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15")
        .Build();
    
    public async Task InitializeAsync()
    {
        await Task.WhenAll(_postgreSqlContainer.StartAsync());

        _requestExecutor = await new ServiceCollection()
            .AddLogging() 
            .AddScoped<IWarehouseService, WarehouseService>()  
            .AddScoped<IWarehouseRepository, WarehouseRepository>()  
            .AddScoped<WarehouseDTOValidator>()
            .AddDbContext<InventoryContext>(
                options => options.UseNpgsql(_postgreSqlContainer.GetConnectionString()))
            .AddGraphQLServer()
            .AddQueryType<WarehouseQueries>()
            .AddFiltering()
            .AddSorting()
            .BuildRequestExecutorAsync();

        var dbContext = _requestExecutor.Services
            .GetApplicationServices()
            .GetRequiredService<InventoryContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}