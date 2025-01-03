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

        var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();

        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied to the database.");
            GenerateFullMigrationScript(dbContext);
        }
        else
        {
            Console.WriteLine("No pending migrations. Skipping migration script generation.");
        }
    }

    private static void GenerateFullMigrationScript(InventoryContext dbContext)
    {
        var migrator = dbContext.Database.GetService<IMigrator>();
        var sqlScript = migrator.GenerateScript(fromMigration: null, toMigration: null);

        var projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;
        var scriptDirectory = Path.Combine(projectRoot!, "src", "Infrastructure", "Data", "Scripts");
        var scriptFileName = "full_migrations_script.sql";
        var scriptPath = Path.Combine(scriptDirectory, scriptFileName);

        Directory.CreateDirectory(Path.GetDirectoryName(scriptPath)!);

        File.WriteAllText(scriptPath, sqlScript);

        Console.WriteLine($"Migration script saved to {scriptPath}");
    }

    private static void GenerateLatestMigrationScript(InventoryContext dbContext)
    {
    }
}