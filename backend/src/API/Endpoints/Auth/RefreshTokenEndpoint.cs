using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

//todo: move to another namespace ot add contract layer
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public static class RefreshTokenEndpoint
{
    private const string Name = "RefreshToken";

    public static IEndpointRouteBuilder MapRefreshToken(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthEndpoints.Refresh,
                async ([FromServices] IAuthenticationService authenticationService,
                    [FromBody] RefreshTokenRequest request,
                    CancellationToken cancellationToken) =>
                {
                    var result = await authenticationService.RefreshTokenAsync(request.RefreshToken);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error =>
                        {
                            return error.Code switch
                            {
                                "Authentication.InvalidRefreshToken" => Results.Unauthorized(),
                                "UserNotFound" => Results.NotFound(new
                                {
                                    type = "https://httpstatuses.com/404",
                                    title = error.Code,
                                    detail = error.Description,
                                    statusCode = StatusCodes.Status404NotFound
                                }),
                                _ => Results.Problem(
                                    type: "https://httpstatuses.com/500",
                                    title: "Internal Server Error",
                                    detail: error.Description,
                                    statusCode: StatusCodes.Status500InternalServerError)
                            };
                        });
                })
            .WithName(Name)
            .WithTags(EndpointTags.Auth)
            .RequireRateLimiting("fixed");

        return app;
    }
}