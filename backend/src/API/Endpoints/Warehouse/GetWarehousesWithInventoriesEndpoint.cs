using API.Authorization;
using API.Endpoints.Constants;
using Application.Interfaces.Services.Warehouse;
using FastEndpoints;

namespace API.Endpoints.Warehouse;

public sealed class
    GetWarehousesWithInventoriesEndpoint : Endpoint<GetWarehouseWithInventoriesRequest, WarehouseWithInventoriesResponse
    ?>
{
    private readonly IWarehouseService _warehouseService;

    public GetWarehousesWithInventoriesEndpoint(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    public override void Configure()
    {
        Get(WarehouseEndpoints.GetWithInventories);
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(GetWarehouseWithInventoriesRequest req, CancellationToken ct)
    {
        var result = await _warehouseService.GetWarehouseWithInventoriesAsync(req.Id, ct);

        if (!result.IsSuccess || result.Value is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var w = result.Value;

        var response = new WarehouseWithInventoriesResponse
        {
            Id = w.Id,
            Name = w.Name,
            Inventories = w.Inventories.Select(i => new InventoryResponse
            {
                Id = i.Id,
                ProdctName = i.ProductName,
                Quantity = i.Quantity
            }).ToList()
        };

        await SendOkAsync(response, ct);
    }
}

public sealed class GetWarehouseWithInventoriesRequest
{
    public Guid Id { get; set; }
}

public sealed class WarehouseWithInventoriesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<InventoryResponse> Inventories { get; set; } = [];
}

public sealed class InventoryResponse
{
    public Guid Id { get; set; }
    public string ProdctName { get; set; }
    public int Quantity { get; set; }
}
