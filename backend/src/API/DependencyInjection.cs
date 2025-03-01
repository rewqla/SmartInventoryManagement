using API.Endpoints;
using API.GraphQL.Mutations;
using API.GraphQL.Queries;
using API.GraphQL.Subscriptions;
using API.Health;
using Application.Authentication;
using Application.DTO.Warehouse;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Application.Interfaces.Services.Warehouse;
using Application.Reports;
using Application.Services;
using Application.Services.Authentication;
using Application.Services.Product;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;
        var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionStrings")!;
        
        services.AddSwagger();
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IReportService<WarehouseDTO>, WarehouseReportService>();
        services.AddScoped<IReportService<Product>, ProductReportService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));


        return builder;
    }

    public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;
        var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionStrings")!;
        
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("postgresql-custom-check")
            .AddNpgSql(connectionString);

        return builder;
    }

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

        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;
        var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionStrings")!;
        
        services.AddDbContext<InventoryContext>(
            options => options.UseNpgsql(connectionString));

        return builder;
    }

    public static WebApplicationBuilder ConfigureGraphQL(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddGraphQLServer()
            .AddQueryType()
            .AddTypeExtension<WarehouseQueries>()
            .AddTypeExtension<WarehouseByContextQueries>()
            .AddMutationType()
            .AddTypeExtension<WarehouseMutations>()
            .AddMutationConventions()
            .AddSubscriptionType()
            .AddTypeExtension<WarehouseSubscriptions>()
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