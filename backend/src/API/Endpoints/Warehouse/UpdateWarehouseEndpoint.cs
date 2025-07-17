using API.Endpoints.Constants;
using API.Extensions;
using API.Hubs;
using Application.DTO.Authentication;
using Application.DTO.Inventory;
using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.Warehouse;

// public static class UpdateWarehouseEndpoint
// {
//     private const string Name = "UpdateWarehouse";
//
//     public static IEndpointRouteBuilder MapUpdateWarehouse(this IEndpointRouteBuilder app)
//     {
//         app.MapPut(WarehouseEndpoints.Update,
//                 async (Guid id, [FromBody] WarehouseDTO warehouseDto, [FromServices] IWarehouseService warehouseService,
//                     [FromServices] IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext, CancellationToken cancellationToken) =>
//                 {
//                     warehouseDto.Id = id;
//                     var result = await warehouseService.UpdateWarehouseAsync(warehouseDto, cancellationToken);
//
//                     if (result.IsSuccess)
//                     {
//                         await hubContext.Clients.All.NotifyWarehouseUpdatedAsync(result.Value);
//                     }
//                     
//                     return result.Match(
//                         onSuccess: value => Results.Ok(value),
//                         onFailure: error =>
//                         {
//                             return error.Code switch
//                             {
//                                 "Warehouse.NotFound" => Results.Problem(
//                                     type: "https://httpstatuses.com/404",
//                                     title: error.Code,
//                                     detail: error.Description,
//                                     statusCode: StatusCodes.Status404NotFound
//                                 ),
//                                 "Warehouse.ValidationError" => Results.Problem(type: "Bad Request",
//                                     title: error.Code,
//                                     detail: error.Description,
//                                     statusCode: StatusCodes.Status400BadRequest,
//                                     extensions: new Dictionary<string, object?>
//                                     {
//                                         { "errors", error.Errors }
//                                     })
//                             };
//                         });
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Warehouse);
//
//         return app;
//     }
// }
public class WarehouseUpdateRequest
{
    [FromRoute]
    public Guid Id { get; set; }

    // Include only the properties that should be updatable
    public string Name { get; set; } = default!;
    public string Location { get; set; } = default!;
    public List<InventoryDTO> Inventories { get; set; } = new();
}

internal sealed class
    UpdateWarehouseEndpoint : Endpoint<WarehouseUpdateRequest, Results<Ok<WarehouseDTO>, NotFound, BadRequest>>
{
    public IWarehouseService warehouseService { get; set; }
    public IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext { get; set; }

    public override void Configure()
    {
        Put(WarehouseEndpoints.Update);
        Description(x => x.WithTags(EndpointTags.Warehouse));
        AllowAnonymous();
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(WarehouseUpdateRequest request, CancellationToken cancellationToken)
    {
        var dto = new WarehouseDTO
        {
            Id = request.Id,
            Name = request.Name,
            Location = request.Location,
            Inventories = request.Inventories
        };
        
        var result = await warehouseService.UpdateWarehouseAsync(dto, cancellationToken);

        if (result.IsSuccess)
        {
            await hubContext.Clients.All.NotifyWarehouseUpdatedAsync(result.Value);

            await SendResultAsync(TypedResults.Ok(result.Value));

            return;
        }

        //todo: update check not by string, but as object

        var error = result.Error;

        switch (error.Code)
        {
            case "Warehouse.NotFound":
                await SendResultAsync(TypedResults.NotFound(new
                {
                    type = "https://httpstatuses.com/404",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status404NotFound
                }));
                break;

            case "Warehouse.ValidationError":
                await SendResultAsync(TypedResults.BadRequest(new
                {
                    type = "https://httpstatuses.com/400",
                    title = error.Code,
                    detail = error.Description,
                    statusCode = StatusCodes.Status400BadRequest,
                    extensions = new Dictionary<string, object?>
                    {
                        { "errors", error.Errors }
                    }
                }));
                break;
        }
    }
}