﻿using API.Endpoints.Constants;
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
                            return error.Code switch
                            {
                                "Warehouse.NotFound" => Results.Problem(
                                    type: "https://httpstatuses.com/404",
                                    title: error.Code,
                                    detail: error.Description,
                                    statusCode: StatusCodes.Status404NotFound
                                ),
                                "Warehouse.ValidationError" => Results.Problem(type: "Bad Request",
                                    title: error.Code,
                                    detail: error.Description,
                                    statusCode: StatusCodes.Status400BadRequest,
                                    extensions: new Dictionary<string, object?>
                                    {
                                        { "errors", error.Errors }
                                    })
                            };
                        });
                })
            .WithName(Name)
            .WithTags(EndpointTags.Warehouse);

        return app;
    }
}