using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Application.Interfaces.News;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.News;

public static class GetTitlesEndpoint
{
    private const string Name = "GetTitles";

    public static IEndpointRouteBuilder MapGetTitles(this IEndpointRouteBuilder app)
    {
        app.MapPost(NewsEndpoints.Titles,
                async ([FromServices] INewsService newsService, [FromQuery] string query,
                    CancellationToken cancellationToken) =>
                {
                    var result = await newsService.GetTopHeadlinesTitlesAsync(query, cancellationToken);

                    return Results.Ok(result);
                })
            .WithName(Name)
            .WithTags(EndpointTags.News);

        return app;
    }
}