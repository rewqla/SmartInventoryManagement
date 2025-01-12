using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

public static class DeleteWarehouseEndpoint
{
    private const string Name = "DeleteWarehouse";

    public static IEndpointRouteBuilder MapDeleteWarehouse(this IEndpointRouteBuilder app)
    {
        app.MapDelete(WarehouseEndpoints.Update,
                async (Guid id, [FromServices] IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.DeleteWarehouse(id, cancellationToken);

                    return result.Match(
                        onSuccess: value => Results.NoContent(),
                        onFailure: error =>
                        {
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