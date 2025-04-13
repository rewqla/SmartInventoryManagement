using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using SmartInventoryManagement.IntegrationTests.Helpers;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests;

public class IntegrationTestWebAppFactory
    : WebApplicationFactory<ISmartInventoryHost>,
        IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptorType =
                typeof(DbContextOptions<InventoryContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<InventoryContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();

        await dbContext.Database.MigrateAsync();

        await SeedDataAsync(dbContext);
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    private async Task SeedDataAsync(InventoryContext dbContext)
    {
        var warehouses = new List<Warehouse>
        {
            new Warehouse
            {
                Id = Guid.Parse("089a905d-660d-46d3-97b5-2933747387bc"), Name = "Warehouse A", Location = "New York"
            },
            new Warehouse
            {
                Id = Guid.Parse("b24ab279-1fd3-4fb0-9107-8563612aee1f"), Name = "Warehouse B", Location = "Rivne"
            },
            new Warehouse { Id = GuidV7.NewGuid(), Name = "Warehouse C", Location = "Berlin" },
            new Warehouse { Id = GuidV7.NewGuid(), Name = "Warehouse D", Location = "Tokyo" },
            new Warehouse { Id = GuidV7.NewGuid(), Name = "Warehouse E", Location = "London" },
        };

        await dbContext.Warehouses.AddRangeAsync(warehouses);
        await dbContext.SaveChangesAsync();
        
        var testRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Worker"
        };
        await dbContext.Roles.AddAsync(testRole);
        
        string password = "Test@1234";
        string hashedPassword = PasswordHasher.Hash(password); 

        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "testuser@example.com",
            PhoneNumber = "+1234567890",
            FullName = "Test User",
            PasswordHash = hashedPassword,
            Role = testRole
        };
        await dbContext.Users.AddAsync(testUser);

        await dbContext.SaveChangesAsync();
    }
}