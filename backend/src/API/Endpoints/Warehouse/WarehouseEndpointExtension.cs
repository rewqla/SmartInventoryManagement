namespace API.Endpoints.Warehouse;

public static class WarehouseEndpointExtension
{
    public static IEndpointRouteBuilder MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetWarehouses();
        // #todo create add, update, delete, get by id warehouse endpoints
        return app;
    }
}