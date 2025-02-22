using System.Diagnostics;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Path = System.IO.Path;

namespace API.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();

        var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();

        if (pendingMigrations.Count != 0)
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied to the database.");
        }
        else
        {
            Console.WriteLine("No pending migrations.");
        }
    }

    internal static void GenerateMigrationScripts(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryContext>();

        var appliedMigrations = dbContext.Database.GetAppliedMigrations().ToList();
        
        if (appliedMigrations.Count < 2)
        {
            Console.WriteLine("Not enough migrations for incremental script. Generating full script only.");
            GenerateFullMigrationScript(dbContext);
            return;
        }

        GenerateFullMigrationScript(dbContext);
        GenerateLatestMigrationScript(dbContext);
    }

    private static void GenerateFullMigrationScript(InventoryContext dbContext)
    {
        var migrator = dbContext.Database.GetService<IMigrator>();
        var sqlScript = migrator.GenerateScript(fromMigration: null, toMigration: null);

        var scriptPath = Path.Combine(GetScriptDirectory(), "full_migrations_script.sql");
        File.WriteAllText(scriptPath, sqlScript);

        Console.WriteLine($"Full migration script saved to {scriptPath}");
    }

    private static void GenerateLatestMigrationScript(InventoryContext dbContext)
    {
        var migrator = dbContext.Database.GetService<IMigrator>();
        var appliedMigrations = dbContext.Database.GetAppliedMigrations().ToList();

        var previousMigration = appliedMigrations[^2];
        var latestMigration = appliedMigrations[^1];

        var sqlScript = migrator.GenerateScript(fromMigration: previousMigration, toMigration: latestMigration);
        var scriptPath = Path.Combine(GetScriptDirectory(), $"{latestMigration}_script.sql");

        File.WriteAllText(scriptPath, sqlScript);

        Console.WriteLine($"Latest migration script saved to {scriptPath}");
    }

    private static string GetScriptDirectory()
    {
        var projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;
        var scriptDirectory = Path.Combine(projectRoot!, "src", "Infrastructure", "Data", "Scripts");
        Directory.CreateDirectory(scriptDirectory);
        return scriptDirectory;
    }
}