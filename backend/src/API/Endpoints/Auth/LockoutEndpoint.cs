using API.Endpoints.Constants;
using API.Extensions;
using Application.Configuration;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

public static class LockoutEndpoint
{
    private const string Name = "Lockout";

    public static IEndpointRouteBuilder MapLockout(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthEndpoints.Lockout,
                ([FromServices] LockoutConfig settings,
                    [FromBody] LockoutConfig newSettings) =>
                {
                    //todo: make better lockout set
                    //todo: make lockout validation
                    //todo: add endpoint for retrieving current lockout settings
                    settings.MaxFailedAttempts = newSettings.MaxFailedAttempts;
                    settings.LockoutDurationMinutes = newSettings.LockoutDurationMinutes;

                    return Results.Ok(settings);
                })
            .WithName(Name)
            .WithTags(EndpointTags.Auth)
            .RequireAuthorization();

        return app;
    }
}