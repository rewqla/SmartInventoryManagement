using API;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureRepositories();
builder.ConfigureServices();
builder.ConfigureValidators();
builder.ConfigureDatabase();
builder.ConfigureGraphQL();

// todo: add serilog and magnify logging
// todo: add request middleware fpr logging urls and methods

var app = builder.Build();

app.ConfigureMiddlewares();
app.MapEndpoints();
app.ApplyMigrationsAndGenerateScripts();

app.Run();