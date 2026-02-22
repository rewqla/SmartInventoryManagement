using API.Authorization;
using API.Endpoints.Constants;
using API.Extensions;
using Application.Interfaces.Inventories;
using FastEndpoints;
using FluentValidation;
using Infrastructure.Entities;

namespace API.Endpoints.Inventories;

public sealed class CreateInventoryEndpoint : Endpoint<CreateInventoryRequest, CreateInventoryResponse>
{
    private readonly IInventoryService _inventoryService;

    public CreateInventoryEndpoint(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override void Configure()
    {
        Post(InventoryEndpoints.Create);
        Roles(PolicyRoles.Admin, PolicyRoles.Manager, PolicyRoles.Worker);
    }

    public override async Task HandleAsync(CreateInventoryRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();

        var inventory = new Inventory
        {
            ProductId = req.ProductId,
            WarehouseId = req.WarehouseId,
            Quantity = req.Quantity
        };

        var created = await _inventoryService.CreateAsync(inventory,userId, ct);

        await SendAsync(new CreateInventoryResponse
        {
            Id = created.Id,
            ProductId = created.ProductId,
            WarehouseId = created.WarehouseId,
            Quantity = created.Quantity
        }, StatusCodes.Status201Created, ct);
    }
}

public sealed class CreateInventoryRequest
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
public sealed class CreateInventoryResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
}
public sealed class CreateInventoryRequestValidator : Validator<CreateInventoryRequest>
{
    public CreateInventoryRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("WarehouseId is required.");
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity must be zero or positive.");
    }
}
