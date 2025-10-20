using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

// public static class GetWarehousesEndpoint
// {
//     private const string Name = "GetWarehouses";
//
//     //todo: add integration tests
//     public static IEndpointRouteBuilder MapGetWarehouses(this IEndpointRouteBuilder app)
//     {
//         app.MapGet(WarehouseEndpoints.GetAll,
//                 async ([FromServices] IWarehouseService warehouseService, CancellationToken cancellationToken) =>
//                 {
//                     var result = await warehouseService.GetWarehousesAsync(cancellationToken);
//
//                     return Results.Ok(result.Value);
//                 })
//             .WithName(Name)
//             .WithTags(EndpointTags.Warehouse)
//             .AddEndpointFilter<ApiKeyAuthenticationEndpointFilter>();
//
//         return app;
//     }
// }

internal sealed class GetWarehousesEndpoint : EndpointWithoutRequest<IEnumerable<WarehouseDTO>>
{
    public IWarehouseService WarehouseService { get; set; }

    public override void Configure()
    {
        Get(WarehouseEndpoints.GetAll);
        Description(x => x.WithTags(EndpointTags.Warehouse));
        AllowAnonymous();
        PreProcessor<ApiKeyAuthenticationPreProcessor>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await WarehouseService.GetWarehousesAsync(ct);

        await SendOkAsync(result.Value!, ct);
    }
}