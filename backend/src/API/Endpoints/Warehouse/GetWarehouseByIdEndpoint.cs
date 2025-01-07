﻿using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Services.Warehouse;

namespace API.Endpoints.Warehouse;

public static class GetWarehouseByIdEndpoint
{
    private const string Name = "GetWarehouseById";

    public static IEndpointRouteBuilder MapGetWarehouseById(this IEndpointRouteBuilder app)
    {
        app.MapGet(WarehouseEndpoints.GetById,
                async (Guid id, IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.GetWarehouseByIdAsync(id, cancellationToken);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error => Results.NotFound(error));
                })
            .WithName(Name)
            .WithTags("Warehouse");

        return app;
    }
}