using API;
using API.Extensions;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.ConfigureRepositories();
builder.ConfigureServices();
builder.ConfigureValidators();
builder.ConfigureDatabase();
builder.AddHealthChecks();
builder.ConfigureGraphQL();

// todo: add serilog and magnify logging
// todo: add request middleware for logging urls and methods

var app = builder.Build();

app.ConfigureMiddlewares();
app.MapEndpoints();
app.ApplyMigrationsAndGenerateScripts();

app.RunWithGraphQLCommands(args);