using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Application.Interfaces.News;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.News;

// public static class GetTitlesEndpoint
// {
//     private const string Name = "GetTitles";
//
//     public static IEndpointRouteBuilder MapGetTitles(this IEndpointRouteBuilder app)
//     {
//         app.MapPost(NewsEndpoints.Titles,
//                 async ([FromServices] INewsService newsService, [FromQuery] string query,
//                     CancellationToken cancellationToken) =>
//                 {
//                     var result = await newsService.GetTopHeadlinesTitlesAsync(query, cancellationToken);
//
//                     return Results.Ok(result);
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.News);
//
//         return app;
//     }
// }

internal sealed class GetTitlesEndpoint : EndpointWithoutRequest
{
    public INewsService newsService { get; set; }

    public override void Configure()
    {
        Get(NewsEndpoints.Titles);
        Description(x => x.WithTags(EndpointTags.News));
        AllowAnonymous();
    }
    //todo: add another query param for count of titles

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        string query = Query<string>("q");
        var result = await newsService.GetTopHeadlinesTitlesAsync(query, cancellationToken);

        await SendAsync(result, cancellation: cancellationToken);
    }
}