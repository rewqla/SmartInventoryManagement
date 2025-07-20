using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Auth;

// public class RefreshTokenRequest
// {
//     public string RefreshToken { get; set; } = string.Empty;
// }
//
// public static class RefreshTokenEndpoint
// {
//     private const string Name = "RefreshToken";
//
//     public static IEndpointRouteBuilder MapRefreshToken(this IEndpointRouteBuilder app)
//     {
//         app.MapPost(AuthEndpoints.Refresh,
//                 async ([FromServices] IAuthenticationService authenticationService,
//                     [FromBody] RefreshTokenRequest request,
//                     CancellationToken cancellationToken) =>
//                 {
//                     var result = await authenticationService.RefreshTokenAsync(request.RefreshToken);
//
//                     return result.Match(
//                         onSuccess: value => Results.Ok(value),
//                         onFailure: error =>
//                         {
//                             return error.Code switch
//                             {
//                                 "Authentication.InvalidRefreshToken" => Results.Unauthorized(),
//                                 "UserNotFound" => Results.NotFound(new
//                                 {
//                                     type = "https://httpstatuses.com/404",
//                                     title = error.Code,
//                                     detail = error.Description,
//                                     statusCode = StatusCodes.Status404NotFound
//                                 }),
//                                 _ => Results.Problem(
//                                     type: "https://httpstatuses.com/500",
//                                     title: "Internal Server Error",
//                                     detail: error.Description,
//                                     statusCode: StatusCodes.Status500InternalServerError)
//                             };
//                         });
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Auth)
//             .RequireRateLimiting("fixed");
//
//         return app;
//     }
// }

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

internal sealed class
    RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, Results<Ok<AuthenticationDTO>, NotFound>>
{
    public IAuthenticationService authenticationService { get; set; }

    public override void Configure()
    {
        Post(AuthEndpoints.Refresh);
        Description(x => x.WithTags(EndpointTags.Auth));
        Options(x => x.RequireRateLimiting("fixed"));
        AllowAnonymous();
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.RefreshTokenAsync(request.RefreshToken);

        if (result.IsSuccess)
        {
            await SendResultAsync(TypedResults.Ok(result.Value));
            return;
        }

        //todo: update check not by string, but as object

        var error = result.Error;

        switch (error.Code)
        {
            case "User.NotFound":
                await SendResultAsync(TypedResults.NotFound(new
                {
                    type = "https://httpstatuses.com/404",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status404NotFound
                }));
                break;

            case "Authentication.InvalidRefreshToken":
                await SendUnauthorizedAsync(cancellation: cancellationToken);
                break;
        }
    }
}
