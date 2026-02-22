using API.Authorization;
using API.Endpoints.Constants;
using API.Hubs;
using Application.DTO.Inventory;
using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.Warehouse;

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
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(WarehouseUpdateRequest request, CancellationToken cancellationToken)
    {
        var dto = new WarehouseDTO
        {
            Id = request.Id,
            Name = request.Name,
            Location = request.Location
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
                    errors = error.Errors
                }));
                break;
        }
    }
}
