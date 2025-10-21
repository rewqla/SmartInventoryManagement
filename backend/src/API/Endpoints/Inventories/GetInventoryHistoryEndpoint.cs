using API.Endpoints.Constants;
using Application.DTO.Inventory;
using Application.Interfaces.Inventories;
using FastEndpoints;
using Infrastructure.Entities;

namespace API.Endpoints.Inventories;

public sealed class GetInventoryByIdRequest
{
    public Guid Id { get; set; }
}

public sealed class GetInventoryHistoryEndpoint : Endpoint<GetInventoryByIdRequest, IEnumerable<InventoryHistoryDTO>>
{
    private readonly IInventoryService _inventoryService;

    public GetInventoryHistoryEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Get(InventoryEndpoints.GetHistory);
    }

    public override async Task HandleAsync(GetInventoryByIdRequest req, CancellationToken ct)
    {
        var history = await _inventoryService.GetHistoryAsync(req.Id, ct);

        if (!history.Any())
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(history, ct);
    }
}
