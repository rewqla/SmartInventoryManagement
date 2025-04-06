using API;
using API.Extensions;
using QuestPDF.Infrastructure;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureRepositories();
    builder.ConfigureServices();
    builder.ConfigureDatabase();
    builder.ConfigureAuth();
    builder.AddHealthChecks();
    builder.ConfigureRateLimiter();
    builder.ConfigureGraphQL();

    QuestPDF.Settings.License = LicenseType.Community;

    // todo: add serilog and magnify logging
    // todo: add request middleware for logging urls and methods
    // todo: add appsettings.Production.json 

    var app = builder.Build();

    app.ConfigureMiddlewares();
    app.ConfigureScheduler();
    app.MapEndpoints();
    
    app.UseRateLimiter();
    

    app.ApplyMigrations();

    await app.RunAsync();
    // app.RunWithGraphQLCommands(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}
finally
{
}

//todo: add timeout for requests