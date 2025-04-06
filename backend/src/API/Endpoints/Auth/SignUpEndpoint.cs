using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

public static class SignUpEndpoint
{
    private const string Name = "SignUp";

    public static IEndpointRouteBuilder MapSignUp(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthEndpoints.SignUp,
                async ([FromServices] IAuthenticationService authenticationService, [FromBody] SignUpDTO request,
                    CancellationToken cancellationToken) =>
                {
                    var result = await authenticationService.SignUpAsync(request);

                    return result.Match(
                        onSuccess: _ => Results.NoContent(),
                        onFailure: error =>
                        {
                            return error.Code switch
                            {
                                "Authentication.UserAlreadyExists" => Results.Conflict(new
                                {
                                    type = "https://httpstatuses.com/409",
                                    title = error.Code,
                                    detail = error.Description,
                                    statusCode = StatusCodes.Status409Conflict
                                }),
                                "Common.NotFound.role" => Results.NotFound(new
                                {
                                    type = "https://httpstatuses.com/404",
                                    title = error.Code,
                                    detail = error.Description,
                                    statusCode = StatusCodes.Status404NotFound
                                }),
                                "SignUpDTO.ValidationError" => Results.Problem(type: "Bad Request",
                                    title: error.Code,
                                    detail: error.Description,
                                    statusCode: StatusCodes.Status400BadRequest,
                                    extensions: new Dictionary<string, object?>
                                    {
                                        { "errors", error.Errors }
                                    })
                            };
                        });
                })
            .WithName(Name)
            .WithTags(EndpointTags.Auth);

        return app;
    }
}