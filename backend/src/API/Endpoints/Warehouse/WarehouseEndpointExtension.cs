namespace API.Endpoints.Warehouse;

public static class WarehouseEndpointExtension
{
    public static IEndpointRouteBuilder MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        // app.MapGetWarehouses();
        // app.MapGetWarehouseById();
        //todo: move to fast ednpoints
        app.MapCreateWarehouse();
        // app.MapUpdateWarehouse();
        // app.MapDeleteWarehouse();

        return app;
    }
}
