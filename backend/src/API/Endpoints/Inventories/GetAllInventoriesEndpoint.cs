using API.Authorization;
using API.Endpoints.Constants;
using Application.Interfaces.Inventories;
using FastEndpoints;

namespace API.Endpoints.Inventories;

public sealed class GetAllInventoriesEndpoint : EndpointWithoutRequest<IEnumerable<GetInventoryResponse>>
{
    private readonly IInventoryService _inventoryService;

    public GetAllInventoriesEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Get(InventoryEndpoints.GetAll);
        Roles(PolicyRoles.Admin, PolicyRoles.Manager, PolicyRoles.Worker);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var inventories = await _inventoryService.GetAllAsync(ct);

        var response = inventories.Select(i => new GetInventoryResponse
        {
            Id = i.Id,
            ProductId = i.ProductId,
            WarehouseId = i.WarehouseId,
            Quantity = i.Quantity
        });

        await SendOkAsync(response, ct);
    }
}
public sealed class GetInventoryResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
