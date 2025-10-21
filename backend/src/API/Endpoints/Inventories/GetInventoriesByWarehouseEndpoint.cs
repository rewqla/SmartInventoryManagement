using API.Endpoints.Constants;
using Application.DTO.Inventory;
using Application.Interfaces.Inventories;
using Infrastructure.Entities;
using FastEndpoints;

namespace API.Endpoints.Inventories;

public sealed class
    GetInventoriesByWarehouseEndpoint : EndpointWithoutRequest<IEnumerable<InventoryDTO>>
{
    private readonly IInventoryService _inventoryService;

    public GetInventoriesByWarehouseEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Get(InventoryEndpoints.GetByWarehouse);
    }

    public override async Task HandleAsync( CancellationToken ct)
    {
        var warehouseId = Route<Guid>("warehouseId");
        var inventories = await _inventoryService.GetByWarehouseAsync(warehouseId, ct);
        await SendOkAsync(inventories, ct);
    }
}

