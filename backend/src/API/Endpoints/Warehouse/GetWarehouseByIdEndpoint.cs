using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Authentication;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

// public static class GetWarehouseByIdEndpoint
// {
//     private const string Name = "GetWarehouseById";
//
//     public static IEndpointRouteBuilder MapGetWarehouseById(this IEndpointRouteBuilder app)
//     {
//         app.MapGet(WarehouseEndpoints.GetById,
//                 async (Guid id, [FromServices] IWarehouseService warehouseService,
//                     CancellationToken cancellationToken) =>
//                 {
//                     var result = await warehouseService.GetWarehouseByIdAsync(id, cancellationToken);
//
//                     return result.Match(
//                         onSuccess: value => Results.Ok(value),
//                         onFailure: error => Results.Problem(
//                             type: "https://httpstatuses.com/404",
//                             title: error.Code,
//                             detail: error.Description,
//                             statusCode: StatusCodes.Status404NotFound
//                         )
//                     );
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Warehouse)
//             .RequireAuthorization(PolicyRoles.Admin);
//
//         return app;
//     }
// }

internal class GetWarehouseByIdRequest
{
    public Guid Id { get; set; }
}

internal sealed class
    GetWarehouseByIdEndpoint : Endpoint<GetWarehouseByIdRequest, Results<Ok<WarehouseDTO>, NotFound>>
{
    public IWarehouseService warehouseService { get; set; }

    public override void Configure()
    {
        Get(WarehouseEndpoints.GetById);
        Description(x => x.WithTags(EndpointTags.Warehouse));
        Roles(PolicyRoles.Admin);
    }

    public override async Task HandleAsync(GetWarehouseByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await warehouseService.GetWarehouseByIdAsync(request.Id, cancellationToken);

        if (result.IsSuccess)
        {
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