using API.Endpoints.Warehouse;

namespace API.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWarehouseEndpoints();

        return app;
    }
}