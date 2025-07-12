using API.Endpoints.Constants;
using API.Extensions;
using Application.Configuration;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

// public static class GetLockoutEndpoint
// {
//     private const string Name = "SetLockout";
//
//     public static IEndpointRouteBuilder MapGetLockout(this IEndpointRouteBuilder app)
//     {
//         app.MapGet(AuthEndpoints.Lockout,
//                 ([FromServices] LockoutConfig settings) => Results.Ok(settings))
//             .WithName($"{Name}.Get")
//             .WithTags(EndpointTags.Auth)
//             .RequireAuthorization();
//
//         return app;
//     }
// }

internal sealed class GetLockoutEndpoint : EndpointWithoutRequest<LockoutConfig>
{
    public LockoutConfig settings { get; set; }

    public override void Configure()
    {
        Get(AuthEndpoints.Lockout);
        Description(x => x.WithTags("Auth"));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        await SendAsync(settings, cancellation: cancellationToken);
    }
}