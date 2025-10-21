using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Inventories;
using Infrastructure.Entities;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.Inventories;

public sealed class UpdateInventoryEndpoint : Endpoint<UpdateInventoryRequest, UpdateInventoryResponse?>
{
    private readonly IInventoryService _inventoryService;

    public UpdateInventoryEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Put(InventoryEndpoints.Update);
    }

    public override async Task HandleAsync(UpdateInventoryRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var updatedInventory = new Inventory
        {
            Id = req.Id,
            Quantity = req.Quantity
        };

        var result = await _inventoryService.UpdateAsync(req.Id, updatedInventory, userId, ct);

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(new UpdateInventoryResponse
        {
            Quantity = req.Quantity,
            ProductId = result.ProductId,
            WarehouseId = result.WarehouseId,
            Id = result.Id,
        }, ct);
    }
}

public sealed class UpdateInventoryRequest
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
}

public sealed class UpdateInventoryResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}

public sealed class UpdateInventoryRequestValidator : Validator<UpdateInventoryRequest>
{
    public UpdateInventoryRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory ID is required.");
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity must be zero or positive.");
    }
}
