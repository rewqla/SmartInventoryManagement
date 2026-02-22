using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Inventories;
using FastEndpoints;

namespace API.Endpoints.Inventories;

public sealed class DeleteInventoryEndpoint : Endpoint<DeleteInventoryRequest>
{
    private readonly IInventoryService _inventoryService;

    public DeleteInventoryEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Delete(InventoryEndpoints.Delete);
        Roles(PolicyRoles.Admin, PolicyRoles.Manager, PolicyRoles.Worker);
    }

    public override async Task HandleAsync(DeleteInventoryRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var deleted = await _inventoryService.DeleteAsync(req.Id, userId, ct);

        if (!deleted)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}

public sealed class DeleteInventoryRequest
{
    public Guid Id { get; set; }
}
