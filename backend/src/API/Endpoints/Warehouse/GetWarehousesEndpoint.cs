using API.Endpoints.Constants;
using Application.Interfaces.Services.Warehouse;

namespace API.Endpoints.Warehouse;

public static class GetWarehousesEndpoint
{
    private const string Name = "GetWarehouses";
    // #todo: write api level tests
    public static IEndpointRouteBuilder MapGetWarehouses(this IEndpointRouteBuilder app)
    {
        app.MapGet(WarehouseEndpoints.GetAll, async (IWarehouseService warehouseService, CancellationToken cancellationToken) => 
            {
                var result = await warehouseService.GetWarehousesAsync(cancellationToken);

                // #todo: add ResultExtensions class with Match method
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Error);
            })
            .WithName(Name)
            .WithTags("Warehouse");

        return app;
    }
}