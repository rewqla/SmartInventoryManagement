using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

public static class UpdateWarehouseEndpoint
{
    private const string Name = "UpdateWarehouse";

    public static IEndpointRouteBuilder MapUpdateWarehouse(this IEndpointRouteBuilder app)
    {
        app.MapPut(WarehouseEndpoints.Update,
                async (Guid id, [FromBody] WarehouseDTO warehouseDto, [FromServices] IWarehouseService warehouseService,
                    CancellationToken cancellationToken) =>
                {
                    warehouseDto.Id = id;
                    var result = await warehouseService.UpdateWarehouseAsync(warehouseDto, cancellationToken);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error =>
                        {
                            // todo: update return data of errors
                            if (error.Code == "Warehouse.NotFound")
                            {
                                return Results.NotFound(error);
                            }

                            return Results.BadRequest(error);
                        });
                })
            .WithName(Name)
            .WithTags("Warehouse");

        return app;
    }
}