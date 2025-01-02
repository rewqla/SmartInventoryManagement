using System.Diagnostics;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Path = System.IO.Path;

namespace API.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrationsAndGenerateScripts(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();

        ApplyDatabaseMigrations(dbContext);
    
        GenerateMigrationScript(dbContext);
    }

    private static void ApplyDatabaseMigrations(InventoryContext dbContext)
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied to the database.");
    }

    private static void GenerateMigrationScript(InventoryContext dbContext)
    {
        var migrator = dbContext.Database.GetService<IMigrator>();
        var sqlScript = migrator.GenerateScript(fromMigration: null, toMigration: null);

        var scriptPath = Path.Combine(AppContext.BaseDirectory, "Migrations", "LastMigration.sql");

        Directory.CreateDirectory(Path.GetDirectoryName(scriptPath)!);

        File.WriteAllText(scriptPath, sqlScript);

        Console.WriteLine($"Migration script saved to {scriptPath}");
    }
}