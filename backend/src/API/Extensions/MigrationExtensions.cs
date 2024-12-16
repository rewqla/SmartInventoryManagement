using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        using var dbContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();
        
        dbContext.Database.Migrate();
    }
}