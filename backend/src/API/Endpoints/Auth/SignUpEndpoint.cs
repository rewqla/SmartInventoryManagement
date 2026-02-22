using API.Endpoints.Constants;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Auth;

//todo: move to RERP structure (request, response, validation)
internal sealed class
    SignUpEndpoint : Endpoint<SignUpDTO, Results<Ok<AuthenticationResponse>, NotFound, BadRequest>>
{
    public IAuthenticationService authenticationService { get; set; }

    public override void Configure()
    {
        Post(AuthEndpoints.SignUp);
        Description(x => x.WithTags(EndpointTags.Auth));
        AllowAnonymous();
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(SignUpDTO request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.SignUpAsync(request);

        if (result.IsSuccess)
        {
            await SendResultAsync(TypedResults.NoContent());
            return;
        }

        //todo: update check not by string, but as object
        var error = result.Error;

        switch (error.Code)
        {
            case "Authentication.EmailAlreadyExists":
            case "Authentication.PhoneAlreadyExists":
                await SendResultAsync(TypedResults.Conflict(new
                {
                    type = "https://httpstatuses.com/409",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status409Conflict
                }));
                break;

            case "Common.NotFound.role":
                await SendResultAsync(TypedResults.Conflict(new
                {
                    type = "https://httpstatuses.com/404",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status404NotFound
                }));
                break;

            case "SignUpDTO.ValidationError":
                await SendResultAsync(TypedResults.BadRequest(new
                {
                    type = "https://httpstatuses.com/400",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status400BadRequest,
                    extensions = new Dictionary<string, object?>
                    {
                        { "errors", error.Errors }
                    }
                }));
                break;
        }
    }
}
