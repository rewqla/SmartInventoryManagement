﻿using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Auth;

public static class RefreshTokenEndpoint
{
    private const string Name = "RefreshToken";

    public static IEndpointRouteBuilder MapRefreshToken(this IEndpointRouteBuilder app)
    {
        app.MapPost(AuthEndpoints.Refresh,
                async ([FromServices] IAuthenticationService authenticationService, string refreshToken,
                    CancellationToken cancellationToken) =>
                {
                    var result = await authenticationService.RefreshTokenAsync(refreshToken);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error =>
                        {
                            //todo: update returning data
                            return Results.Problem(
                                type: "https://httpstatuses.com/500",
                                title: "Internal Server Error",
                                detail: error.Description,
                                statusCode: StatusCodes.Status500InternalServerError);
                        });
                })
            .WithName(Name)
            .WithTags("Auth");

        //todo: make tag as separate variable

        return app;
    }
}