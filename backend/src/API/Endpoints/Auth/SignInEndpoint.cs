using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Endpoints.Auth;

// public static class SignInEndpoint
// {
//     private const string Name = "SignIn";
//
//     public static IEndpointRouteBuilder MapSignIn(this IEndpointRouteBuilder app)
//     {
//         app.MapPost(AuthEndpoints.SignIn,
//                 async ([FromServices] IAuthenticationService authenticationService, [FromBody] SignInDTO request,
//                     CancellationToken cancellationToken) =>
//                 {
//                     var result = await authenticationService.SignInAsync(request);
//
//                     return result.Match(
//                         onSuccess: value => Results.Ok(value),
//                         onFailure: error =>
//                         {
//                             return error.Code switch
//                             {
//                                 //todo: update check not by string, but as object
//                                 "User.NotFound" => Results.NotFound(new
//                                 {
//                                     type = "https://httpstatuses.com/404",
//                                     title = error.Code,
//                                     detail = error.Description,
//                                     statusCode = StatusCodes.Status404NotFound
//                                 }),
//                                 "Authentication.InvalidCredentials" => Results.Unauthorized(),
//                                 "Authentication.AccountLockedOut" => Results.BadRequest(new
//                                 {
//                                     type = "https://httpstatuses.com/400",
//                                     title = error.Code,
//                                     detail = error.Description,
//                                     statusCode = StatusCodes.Status400BadRequest
//                                 })
//                             };
//                         });
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Auth);
//
//         return app;
//     }
// }

//todo: move to RERP structure (request, response, validation)
internal sealed class
    SignInEndpoint : Endpoint<SignInDTO, Results<Ok<AuthenticationDTO>, NotFound, BadRequest>>
{
    public IAuthenticationService authenticationService { get; set; }

    public override void Configure()
    {
        Post(AuthEndpoints.SignIn);
        Description(x => x.WithTags(EndpointTags.Auth));
        AllowAnonymous();
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(SignInDTO request, CancellationToken cancellationToken)
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
