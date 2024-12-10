using API.GraphQL;
using API.GraphQL.Mutations;
using Infrastructure.Data;
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
            .AddMutationType<Mutation>()
            .AddMutationConventions();

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

        // app.UseAuthentication();
        // app.UseAuthorization();

        return app;
    }
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapGraphQL();

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