using Application.DTO.Product;
using Application.Interfaces.Services.Product;
using FastEndpoints;

namespace API.Endpoints.Products;

public sealed class GetProductsByCategoryEndpoint : Endpoint<GetProductsByCategoryRequest,IEnumerable<ProductDto>>
{
    private readonly IProductService _service;

    public GetProductsByCategoryEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/categories/{categoryId:guid}/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductsByCategoryRequest req, CancellationToken ct)
    {
        var result = await _service.GetByCategoryAsync(req.CategoryId, ct);

        if (result.IsSuccess)
            await SendOkAsync(result.Value, ct);
        else
            await SendErrorsAsync(statusCode: 404, cancellation: ct);
    }
}
public class GetProductsByCategoryRequest
{
    public Guid CategoryId { get; set; }
}
