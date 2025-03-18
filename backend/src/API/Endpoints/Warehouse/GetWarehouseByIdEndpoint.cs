using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

public static class GetWarehouseByIdEndpoint
{
    private const string Name = "GetWarehouseById";

    public static IEndpointRouteBuilder MapGetWarehouseById(this IEndpointRouteBuilder app)
    {
        app.MapGet(WarehouseEndpoints.GetById,
                async (Guid id, [FromServices]  IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.GetWarehouseByIdAsync(id, cancellationToken);
                    
                    return Results.Ok(result.Value);
                })
            .WithName(Name)
            .WithTags("Warehouse")
            .RequireAuthorization(PolicyRoles.Admin);

        return app;
    }
}