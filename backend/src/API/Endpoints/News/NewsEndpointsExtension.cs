namespace API.Endpoints.News;

public static class NewsEndpointsExtension
{
    public static IEndpointRouteBuilder MapNewsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetTitles();

        return app;
    }
}