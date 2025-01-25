using API.Endpoints;
using API.GraphQL;
using API.GraphQL.Mutations;
using API.GraphQL.Queries;
using API.GraphQL.Subscriptions;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Application.Interfaces.Services.Warehouse;
using Application.Reports;
using Application.Services.Product;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories.Warehouse;
using Infrastructure.Repositories.Warehouse;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddSwagger();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IReportService<WarehouseDTO>, WarehouseReportService>();
        services.AddScoped<IReportService<Product>, ProductReportService>();

        return builder;
    }
    // todo: add health checks
    public static WebApplicationBuilder ConfigureValidators(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddScoped<WarehouseDTOValidator>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services
            .AddScoped<IWarehouseRepository, WarehouseRepository>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddDbContext<InventoryContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }

    public static WebApplicationBuilder ConfigureGraphQL(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddTypeExtension<WarehouseQueries>()
            .AddTypeExtension<WarehouseByContextQueries>()
            .AddMutationType<WarehouseMutations>()
            .AddMutationConventions()
            .AddSubscriptionType<WarehouseSubscriptions>()
            .AddInMemorySubscriptions()
            .AddFiltering()
            .AddSorting()
            .AddProjections();

        return builder;
    }

    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseWebSockets();

        // app.UseAuthentication();
        // app.UseAuthorization();

        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapGraphQL();
        app.MapApiEndpoints();

        return app;
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // var jwtSecurityScheme = new OpenApiSecurityScheme
            // {
            //     BearerFormat = "JWT",
            //     Name = "Authorization",
            //     In = ParameterLocation.Header,
            //     Type = SecuritySchemeType.ApiKey,
            //     Scheme = JwtBearerDefaults.AuthenticationScheme,
            //     Description = "Put Bearer [space] and then your token ",
            // };
            //
            // c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
            //
            // var securityRequirement = new OpenApiSecurityRequirement
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference
            //             {
            //                 Type = ReferenceType.SecurityScheme,
            //                 Id = JwtBearerDefaults.AuthenticationScheme
            //             }
            //         },
            //         []
            //     }
            // };
            //
            // c.AddSecurityRequirement(securityRequirement);
        });
    }
}