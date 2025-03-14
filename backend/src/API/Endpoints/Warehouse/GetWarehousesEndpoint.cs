﻿using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

public static class GetWarehousesEndpoint
{
    private const string Name = "GetWarehouses";

    public static IEndpointRouteBuilder MapGetWarehouses(this IEndpointRouteBuilder app)
    {
        app.MapGet(WarehouseEndpoints.GetAll,
                async ([FromServices] IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.GetWarehousesAsync(cancellationToken);

                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error =>
                            Results.Problem(
                                type: "https://httpstatuses.com/500",
                                title: "Internal Server Error",
                                detail: error.Description,
                                statusCode: StatusCodes.Status500InternalServerError)
                    );
                })
            .WithName(Name)
            .WithTags("Warehouse");

        return app;
    }
}