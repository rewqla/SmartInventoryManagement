using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Inventories;
using FastEndpoints;
using FluentValidation;
using Infrastructure.Entities;

namespace API.Endpoints.Inventories;

public sealed class ReleaseInventoryEndpoint : Endpoint<ReleaseInventoryRequest, ReleaseInventoryResponse>
{
    private readonly IInventoryService _inventoryService;

    public ReleaseInventoryEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Post(InventoryEndpoints.Release);
    }

    public override async Task HandleAsync(ReleaseInventoryRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var inventoryId = Route<Guid>("id");

        var updatedInventory = await _inventoryService.ReleaseAsync(inventoryId, req.QuantityToRelease, userId, ct);

        if (updatedInventory is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new ReleaseInventoryResponse
        {
            Id = updatedInventory.Id,
            ProductId = updatedInventory.ProductId,
            WarehouseId = updatedInventory.WarehouseId,
            Quantity = updatedInventory.Quantity
        };

        await SendOkAsync(response, ct);
    }
}

public sealed class ReleaseInventoryRequest
{
    public int QuantityToRelease { get; set; }
}
public sealed class ReleaseInventoryResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
public sealed class ReleaseInventoryRequestValidator : Validator<ReleaseInventoryRequest>
{
    public ReleaseInventoryRequestValidator()
    {
        RuleFor(x => x.QuantityToRelease)
            .GreaterThan(0)
            .WithMessage("Quantity to release must be greater than 0.");
    }
}
