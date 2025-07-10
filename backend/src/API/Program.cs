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
    builder.ConfigureRefit();
    builder.ConfigureSMTPHost();

    QuestPDF.Settings.License = LicenseType.Community;

    // todo: add serilog and magnify logging
    // todo: add request middleware for logging urls and methods
    // todo: add appsettings.Production.json 
    //todo: add timeout for requests
    //todo: move to vertical slice arcitecture

    var app = builder.Build();

    app.ConfigureCors();
    app.ConfigureMiddlewares();
    app.ConfigureScheduler();
    app.ConfigureHubs();
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