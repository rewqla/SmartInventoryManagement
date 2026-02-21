using API.Endpoints.Constants;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Auth;

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
