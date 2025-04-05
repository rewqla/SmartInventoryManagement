using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

public static class SignInEndpoint
{
    private const string Name = "SignIn";

    public static IEndpointRouteBuilder MapSignIn(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthEndpoints.SignIn,
                async ([FromServices] IAuthenticationService authenticationService, [FromBody] SignInDTO request,
                    CancellationToken cancellationToken) =>
                {
                    var result = await authenticationService.SignInAsync(request);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error =>
                        {
                            return error.Code switch
                            {
                                "User.NotFound" => Results.NotFound(new
                                {
                                    type = "https://httpstatuses.com/404",
                                    title = error.Code,
                                    detail = error.Description,
                                    statusCode = StatusCodes.Status404NotFound
                                }),
                                "Authentication.InvalidCredentials" => Results.Unauthorized()
                            };
                        });
                })
            .WithName(Name)
            .WithTags("Auth");

        //todo: make tag as separate variable

        return app;
    }
}