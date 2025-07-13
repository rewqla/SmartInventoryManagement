namespace API.Endpoints.Warehouse;

public static class WarehouseEndpointExtension
{
    public static IEndpointRouteBuilder MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        //todo: move to fast ednpoints
        app.MapGetWarehouses();
        // app.MapGetWarehouseById();
        //todo: move to fast ednpoints
        app.MapCreateWarehouse();
        //todo: move to fast ednpoints
        app.MapUpdateWarehouse();
        //todo: move to fast ednpoints
        app.MapDeleteWarehouse();
        
        return app;
    }
}