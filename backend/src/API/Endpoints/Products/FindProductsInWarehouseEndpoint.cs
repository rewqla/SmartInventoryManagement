using API.Authorization;
using Application.DTO.Product;
using Application.Interfaces.Services.Product;
using FastEndpoints;

namespace API.Endpoints.Products;

public sealed class
    FindProductsInWarehouseEndpoint : Endpoint<FindProductsInWarehouseRequest,
    IEnumerable<ShortProductDto>>
{
    private readonly IProductService _service;

    public FindProductsInWarehouseEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/warehouses/{warehouseId:guid}/products");
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(FindProductsInWarehouseRequest request, CancellationToken ct)
    {
        var result = await _service.FindInWarehousesAsync(request.WarehouseId, ct);

        if (result.IsSuccess)
            await SendOkAsync(result.Value, ct);
        else
        {
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
}

public class FindProductsInWarehouseRequest
{
    public Guid WarehouseId { get; set; }
}
