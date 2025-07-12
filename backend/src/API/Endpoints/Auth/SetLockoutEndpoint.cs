using API.Endpoints.Constants;
using API.Extensions;
using Application.Configuration;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

// public static class SetLockoutEndpoint
// {
//     private const string Name = "SetLockout";
//
//     public static IEndpointRouteBuilder MapSetLockout(this IEndpointRouteBuilder app)
//     {
//         app.MapPost(AuthEndpoints.Lockout,
//                 ([FromServices] LockoutConfig settings,
//                     [FromBody] LockoutConfig newSettings) =>
//                 {
//                    
//                     
//                     settings.MaxFailedAttempts = newSettings.MaxFailedAttempts;
//                     settings.LockoutDurationMinutes = newSettings.LockoutDurationMinutes;
//
//                     return Results.Ok(settings);
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Auth)
//             .RequireAuthorization();
//
//         return app;
//     }
// }

internal class SetLockoutRequest
{
    public int MaxFailedAttempts { get; set; }
    public int LockoutDurationMinutes { get; set; }
}

internal sealed class SetLockoutEndpoint : Endpoint<SetLockoutRequest, LockoutConfig>
{
    public LockoutConfig settings { get; set; }

    public override void Configure()
    {
        Post(AuthEndpoints.Lockout);
        Description(x => x.WithTags("Auth"));
    }
    
    //todo: make lockout validation
    //todo: make better lockout set
    public override async Task HandleAsync(SetLockoutRequest setLockoutRequest, CancellationToken cancellationToken)
    {
        settings.MaxFailedAttempts = setLockoutRequest.MaxFailedAttempts;
        settings.LockoutDurationMinutes = setLockoutRequest.LockoutDurationMinutes;
        
        await SendAsync(settings, cancellation: cancellationToken);
    }
}