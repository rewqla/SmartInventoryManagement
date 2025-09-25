using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using API.Hubs;
using FastEndpoints;
using Application.Interfaces.Hubs;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.Warehouse;

// public static class DeleteWarehouseEndpoint
// {
//     private const string Name = "DeleteWarehouse";
//
//     public static IEndpointRouteBuilder MapDeleteWarehouse(this IEndpointRouteBuilder app)
//     {
//         app.MapDelete(WarehouseEndpoints.Delete,
//                 async (Guid id,
//                     [FromServices] IWarehouseService warehouseService,
//                     [FromServices] IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext,
//                     CancellationToken cancellationToken) =>
//                 {
//                     var result = await warehouseService.DeleteWarehouse(id, cancellationToken);
//
//                     if (result.IsSuccess)
//                     {
//                         await hubContext.Clients.All.NotifyWarehouseDeletedAsync();
//                     }
//
//                     return result.Match(
//                         onSuccess: value => Results.NoContent(),
//                         onFailure: error => Results.Problem(
//                             type: "https://httpstatuses.com/404",
//                             title: error.Code,
//                             detail: error.Description,
//                             statusCode: StatusCodes.Status404NotFound
//                         )
//                     );
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Warehouse);
//         return app;
//     }
// }

internal class DeleteWarehouseRequest
{
    public Guid Id { get; set; }
}

internal sealed class
    DeleteWarehouseEndpoint : Endpoint<DeleteWarehouseRequest, Results<NoContent, NotFound>>
{
    public IWarehouseService warehouseService { get; set; }
    public IHubContext<WarehouseNotificationHub, IWarehouseNotificationClient> hubContext { get; set; }

    public override void Configure()
    {
        Delete(WarehouseEndpoints.Delete);
        Description(x => x.WithTags(EndpointTags.Warehouse));
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteWarehouseRequest request, CancellationToken cancellationToken)
    {
        var result = await warehouseService.DeleteWarehouse(request.Id, cancellationToken);

        if (result.IsSuccess)
        {
            await hubContext.Clients.All.NotifyWarehouseDeletedAsync();
            await SendResultAsync(TypedResults.Ok(result.Value));

            return;
        }

        var error = result.Error;

        await SendResultAsync(TypedResults.NotFound(new
        {
            type = "https://httpstatuses.com/404",
            title = error.Code,
            detail = error.Description,
            statusCode = StatusCodes.Status404NotFound
        }));
    }
}
