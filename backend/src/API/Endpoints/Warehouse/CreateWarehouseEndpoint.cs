using API.Authorization;
using API.Endpoints.Constants;
using API.Hubs;
using FastEndpoints;
using Application.DTO.Inventory;
using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.Warehouse;

//todo: write integration tests using fast endpoints extension

public class WarehouseCreateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Location { get; set; } = default!;
    public List<InventoryDTO> Inventories { get; set; } = [];
}

internal sealed class
    CreateWarehouseEndpoint : Endpoint<WarehouseCreateRequest, Results<Ok<WarehouseDTO>, NotFound, BadRequest>>
{
    public IWarehouseService warehouseService { get; set; }
    public IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext { get; set; }

    public override void Configure()
    {
        Post(WarehouseEndpoints.Create);
        Description(x => x.WithTags(EndpointTags.Warehouse));
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    //todo: think about refactoring of returning types
    public override async Task HandleAsync(WarehouseCreateRequest request, CancellationToken cancellationToken)
    {
        var dto = new WarehouseDTO
        {
            Id = request.Id,
            Name = request.Name,
            Location = request.Location
        };

        var result = await warehouseService.CreateWarehouseAsync(dto, cancellationToken);

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
