using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

public static class CreateWarehouseEndpoint
{
    private const string Name = "CreateWarehouse";

    public static IEndpointRouteBuilder MapCreateWarehouse(this IEndpointRouteBuilder app)
    {
        app.MapPost(WarehouseEndpoints.Create,
                async ([FromBody] WarehouseDTO warehouseDto, [FromServices] IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.CreateWarehouseAsync(warehouseDto, cancellationToken);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error => Results.BadRequest(error));
                })
            .WithName(Name)
            .WithTags("Warehouse");

        return app;
    }
}