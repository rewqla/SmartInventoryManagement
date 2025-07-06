using API.Endpoints.Constants;
using API.Extensions;
using API.Hubs;
using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.Warehouse;

public static class DeleteWarehouseEndpoint
{
    private const string Name = "DeleteWarehouse";

    public static IEndpointRouteBuilder MapDeleteWarehouse(this IEndpointRouteBuilder app)
    {
        app.MapDelete(WarehouseEndpoints.Delete,
                async (Guid id,
                    [FromServices] IWarehouseService warehouseService,
                    [FromServices] IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext,
                    CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.DeleteWarehouse(id, cancellationToken);

                    if (result.IsSuccess)
                    {
                        await hubContext.Clients.All.NotifyWarehouseDeletedAsync();
                    }
                    
                    return result.Match(
                        onSuccess: value => Results.NoContent(),
                        onFailure: error => Results.Problem(
                            type: "https://httpstatuses.com/404",
                            title: error.Code,
                            detail: error.Description,
                            statusCode: StatusCodes.Status404NotFound
                        )
                    );
                })
            .WithName(Name)
            .WithTags(EndpointTags.Warehouse);
        return app;
    }
}