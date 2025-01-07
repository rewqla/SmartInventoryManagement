namespace API.Endpoints.Warehouse;

public static class WarehouseEndpointExtension
{
    public static IEndpointRouteBuilder MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetWarehouses();
        app.MapGetWarehouseById();
        // #todo create add, update, delete warehouse endpoints
        return app;
    }
}