using API.Authorization;
using Application.DTO.Product;
using Application.Interfaces.Services.Product;
using FastEndpoints;

namespace API.Endpoints.Products;

public sealed class GetAllProductsEndpoint : EndpointWithoutRequest<IEnumerable<ProductDto>>
{
    private readonly IProductService _service;

    public GetAllProductsEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/products");
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);

        if (result.IsSuccess)
            await SendOkAsync(result.Value!, ct);
        else
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
    }
}
