using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using API.Authorization;
using API.Endpoints;
using API.GraphQL.Mutations;
using API.GraphQL.Queries;
using API.GraphQL.Subscriptions;
using API.Health;
using Application.Authentication;
using Application.BackgroundJobs;
using Application.DTO.Warehouse;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Application.Interfaces.Services.Warehouse;
using Application.Reports;
using Application.Services;
using Application.Services.Product;
using Application.Services.Warehouse;
using Application.Validation.Warehouse;
using Coravel;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Application.Configuration;
using Application.Interfaces.News;
using Application.Services.News;
using Refit;


namespace API;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;
        var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionStrings")!;

        services.AddSwagger();
        services.AddScheduler();
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

        services.AddSingleton<LockoutConfig>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));

        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<INewsService, NewsService>();

        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportGenerator<WarehouseDTO>, WarehouseReportGenerator>();
        services.AddScoped<IReportGenerator<Product>, ProductReportGenerator>();

        services.AddTransient<CleanupExpiredTokensJob>();

        services.AddValidatorsFromAssembly(typeof(WarehouseDTOValidator).Assembly);

        return builder;
    }

    public static WebApplicationBuilder ConfigureRefit(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        var newsApiKey = builder.Configuration.GetValue<string>("NewsApiKey")!;

        services.AddRefitClient<INewsApi>()
            .ConfigureHttpClient(
                c =>
                {
                    c.BaseAddress = new Uri("https://newsapi.org/v2");
                    c.DefaultRequestHeaders.Add("Authorization", newsApiKey);
                    c.DefaultRequestHeaders.Add("User-Agent", "Dj");
                    // c.DefaultRequestHeaders.Add("X-Api-Key", newsApiKey);
                }
            );

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyRoles.Admin, policy =>
                policy.RequireRole(PolicyRoles.Admin));
            options.AddPolicy(PolicyRoles.Manager, policy =>
                policy.RequireRole(PolicyRoles.Manager));

            options.AddPolicy(PolicyRoles.Worker, policy =>
                policy.RequireRole(PolicyRoles.Worker));
        });

        services.Configure<JwtOptions>(
            builder.Configuration.GetSection("Jwt"));

        return builder;
    }

    public static WebApplicationBuilder ConfigureRateLimiter(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.AddPolicy("fixed", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });

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

    public static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var services = builder.Services;

        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return builder;
    }

    public static WebApplication ConfigureScheduler(this WebApplication app)
    {
        app.Services.UseScheduler(scheduler =>
        {
            scheduler.Schedule<CleanupExpiredTokensJob>()
                .DailyAt(2, 00);
        });
        return app;
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

        app.UseAuthentication();
        app.UseAuthorization();

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
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put Bearer [space] and then your token ",
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            c.AddSecurityRequirement(securityRequirement);
        });
    }
}