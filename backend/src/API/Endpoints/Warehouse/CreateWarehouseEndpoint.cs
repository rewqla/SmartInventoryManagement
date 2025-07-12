using API.Endpoints.Constants;
using API.Extensions;
using API.Hubs;
using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.Warehouse;

public static class CreateWarehouseEndpoint
{
    private const string Name = "CreateWarehouse";

    public static IEndpointRouteBuilder MapCreateWarehouse(this IEndpointRouteBuilder app)
    {
        app.MapPost(WarehouseEndpoints.Create,
                async ([FromBody] WarehouseDTO warehouseDto, [FromServices] IWarehouseService warehouseService,
                    [FromServices] IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext,
                    CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.CreateWarehouseAsync(warehouseDto, cancellationToken);

                    if (result.IsSuccess)
                    {
                        await hubContext.Clients.All.NotifyWarehouseAddedAsync(result.Value);
                    }
                    
                    return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error =>
                        {
                            return error.Code switch
                            {
                                "Warehouse.ValidationError" => Results.Problem(type: "Bad Request",
                                    title: error.Code,
                                    detail: error.Description,
                                    statusCode: StatusCodes.Status400BadRequest,
                                    extensions: new Dictionary<string, object?>
                                    {
                                        { "errors", error.Errors }
                                    }),
                                _ => Results.Problem(
                                    type: "https://httpstatuses.com/500",
                                    title: "Internal Server Error",
                                    detail: error.Description,
                                    statusCode: StatusCodes.Status500InternalServerError)
                            };
                        });
                })
            .WithName(Name)
            .WithTags(EndpointTags.Warehouse);

        return app;
    }
}

//todo: write integration tests using fast endpoints extension