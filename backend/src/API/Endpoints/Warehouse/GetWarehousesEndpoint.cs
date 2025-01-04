using API.Endpoints.Constants;
using Application.Interfaces.Services.Warehouse;

namespace API.Endpoints.Warehouse;

public static class GetWarehousesEndpoint
{
    private const string Name = "GetWarehouses";

    public static IEndpointRouteBuilder MapGetWarehouses(this IEndpointRouteBuilder app)
    {
        app.MapGet(WarehouseEndpoints.GetAll, async (IWarehouseService countryService, CancellationToken cancellationToken) => 
            {
                var countries = await countryService.GetWarehousesAsync(cancellationToken);

                return TypedResults.Ok(countries);
            })
            .WithName(Name)
            .WithTags("Country");

        return app;
    }
}