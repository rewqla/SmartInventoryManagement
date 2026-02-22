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
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
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
