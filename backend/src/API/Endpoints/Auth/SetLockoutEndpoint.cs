using API.Endpoints.Constants;
using API.Extensions;
using Application.Configuration;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

public static class SetLockoutEndpoint
{
    private const string Name = "SetLockout";

    public static IEndpointRouteBuilder MapSetLockout(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthEndpoints.Lockout,
                ([FromServices] LockoutConfig settings,
                    [FromBody] LockoutConfig newSettings) =>
                {
                    //todo: make better lockout set
                    //todo: make lockout validation
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