using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.DTO.Warehouse;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Warehouse;

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
