using API.GraphQL.Queries;
using Application.Interfaces.Services.Warehouse;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
using HotChocolate;
using HotChocolate.Execution;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Infrastructure.Repositories.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Snapshooter.Xunit;

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
        
        await SeedDataAsync(dbContext);
    }
    
    private async Task SeedDataAsync(InventoryContext dbContext)
    {
        // Check if the database is empty
        if (!dbContext.Warehouses.Any())
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse { Id = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc"), Name = "Warehouse A", Location = "New York" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse B", Location = "Rivne" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse C", Location = "Berlin" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse D", Location = "Tokyo" },
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse E", Location = "London" },
            };

            await dbContext.Warehouses.AddRangeAsync(warehouses);
            await dbContext.SaveChangesAsync();
            
        }
    }
    
    // [Fact]
    public async Task GetWarehouses_ReturnsPaginatedResults_WithSorting()
    {
        // Arrange & act
        IExecutionResult result = await _requestExecutor.ExecuteAsync(
            @"
                 query{
                   warehouse (take: 5, skip: 0) {
                     items{
                       name
                       location
                     }
                     pageInfo{
                       hasNextPage
                       hasPreviousPage
                     }
                     totalCount
                   }
                 }
                ");

        // assert
        result.ToJson().MatchSnapshot();
    }
    [Fact]
    public async Task GetWarehouseById_ReturnWarehouse_WhenExists()
    {
        // Arrange & act
        IExecutionResult result = await _requestExecutor.ExecuteAsync(
            @"
                query{
                  warehouseById(id: ""089a905d-660d-46d3-97b5-2933747387bc""){
                    id
                    name
                    location
                  }
                }
                ");

        // assert
        result.ToJson().MatchSnapshot();
    }
    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}