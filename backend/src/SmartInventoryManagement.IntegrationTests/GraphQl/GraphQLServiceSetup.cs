﻿namespace SmartInventoryManagement.IntegrationTests.GraphQl;

public class GraphQLServiceSetup: IAsyncLifetime
{
    public IRequestExecutor RequestExecutor { get; private set; } = null!;
    
    private readonly PostgreSqlContainer _postgresqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15")
        .Build();

    public async Task InitializeAsync()
    { 
        await _postgresqlContainer.StartAsync();
        
        var services = new ServiceCollection()
            .AddLogging()
            .AddScoped<IWarehouseService, WarehouseService>()
            .AddScoped<IReportService<WarehouseDTO>, WarehouseReportService>()
            .AddScoped<IWarehouseRepository, WarehouseRepository>()
            .AddScoped<WarehouseDTOValidator>()
            .AddDbContext<InventoryContext>(options =>
                options.UseNpgsql(_postgresqlContainer.GetConnectionString()))
            .AddGraphQLServer()
            .AddQueryType()
            .AddTypeExtension<WarehouseQueries>()
            .AddMutationType()
            .AddTypeExtension<WarehouseMutations>()
            .AddMutationConventions()
            .AddSubscriptionType()
            .AddTypeExtension<WarehouseSubscriptions>()
            .AddInMemorySubscriptions()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        var executor = await services.BuildRequestExecutorAsync();

        RequestExecutor = executor;
        
        var dbContext = RequestExecutor.Services
            .GetApplicationServices()
            .GetRequiredService<InventoryContext>();

        await dbContext.Database.EnsureCreatedAsync();
        
        await SeedDataAsync(dbContext);
    }
    // todo: add other seeds
    private async Task SeedDataAsync(InventoryContext dbContext)
    {
        if (!dbContext.Warehouses.Any())
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse { Id = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc"), Name = "Warehouse A", Location = "New York" },
                new Warehouse { Id = Guid.Parse("b24ab279-1fd3-4fb0-9107-8563612aee1f"), Name = "Warehouse B", Location = "Rivne" },
                new Warehouse { Id = GuidV7.NewGuid(), Name = "Warehouse C", Location = "Berlin" },
                new Warehouse { Id = GuidV7.NewGuid(), Name = "Warehouse D", Location = "Tokyo" },
                new Warehouse { Id = GuidV7.NewGuid(), Name = "Warehouse E", Location = "London" },
            };

            await dbContext.Warehouses.AddRangeAsync(warehouses);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task DisposeAsync()
    {
        await _postgresqlContainer.DisposeAsync();
    }
}