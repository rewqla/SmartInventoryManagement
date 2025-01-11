namespace API.Endpoints.Warehouse;

public static class WarehouseEndpointExtension
{
    public static IEndpointRouteBuilder MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetWarehouses();
        app.MapGetWarehouseById();
        app.MapCreateWarehouse();
        app.MapUpdateWarehouse();
        
        // #todo delete warehouse endpoints
        return app;
    }
}