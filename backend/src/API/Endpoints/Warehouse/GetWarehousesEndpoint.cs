using API.Endpoints.Constants;
using API.Extensions;
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

                return result.Match(
                    onSuccess: value => Results.Ok(value),
                    onFailure: error => Results.BadRequest(error));
            })
            .WithName(Name)
            .WithTags("Warehouse");

        return app;
    }
}