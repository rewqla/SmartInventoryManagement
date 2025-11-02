using Application.DTO.Product;
using Application.Interfaces.Services.Product;
using FastEndpoints;

namespace API.Endpoints.Products;

public sealed class CreateProductEndpoint : Endpoint<MutationProductDto, ProductDto>
{
    private readonly IProductService _service;

    public CreateProductEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MutationProductDto req, CancellationToken ct)
    {
        var result = await _service.CreateAsync(req, ct);

        if (result.IsSuccess)
            await SendOkAsync(
                result.Value!,
                cancellation: ct
            );
        else
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
    }
}

