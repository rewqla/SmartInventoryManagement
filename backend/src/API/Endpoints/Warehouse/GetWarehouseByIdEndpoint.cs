using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

public static class GetWarehouseByIdEndpoint
{
    private const string Name = "GetWarehouseById";

    public static IEndpointRouteBuilder MapGetWarehouseById(this IEndpointRouteBuilder app)
    {
        app.MapGet(WarehouseEndpoints.GetById,
                async (Guid id, [FromServices]  IWarehouseService warehouseService, CancellationToken cancellationToken) =>
                {
                    var result = await warehouseService.GetWarehouseByIdAsync(id, cancellationToken);
                    
                     return result.Match(
                        onSuccess: value => Results.Ok(value),
                        onFailure: error => Results.Problem(
                            type: "https://httpstatuses.com/404",
                            title: error.Code,
                            detail: error.Description,
                            statusCode: StatusCodes.Status404NotFound
                        )
                    );
                })
            .WithName(Name)
            .WithTags("Warehouse")
            .RequireAuthorization(PolicyRoles.Admin);

        return app;
    }
}