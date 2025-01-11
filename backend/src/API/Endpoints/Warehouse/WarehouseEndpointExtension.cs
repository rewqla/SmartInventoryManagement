namespace API.Endpoints.Warehouse;

public static class WarehouseEndpointExtension
{
    public static IEndpointRouteBuilder MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetWarehouses();
        app.MapGetWarehouseById();
        app.MapCreateWarehouse();
        
        // #todo update, delete warehouse endpoints
        return app;
    }
}