using API.Endpoints.Constants;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Auth;


//todo: move to RERP structure (request, response, validation)
internal sealed class
    SignInEndpoint : Endpoint<SignInRequest, Results<Ok<AuthenticationResponse>, NotFound, BadRequest>>
{
    public IAuthenticationService authenticationService { get; set; }

    public override void Configure()
    {
        Post(AuthEndpoints.SignIn);
        Description(x => x.WithTags(EndpointTags.Auth));
        AllowAnonymous();
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(SignInRequest request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.SignInAsync(request);

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

            case "Authentication.InvalidCredentials":
                await SendUnauthorizedAsync(cancellation: cancellationToken);
                break;

            case "Authentication.AccountLockedOut":
                await SendResultAsync(TypedResults.BadRequest(new
                {
                    type = "https://httpstatuses.com/400",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status400BadRequest
                }));
                break;
        }
    }
}
