using API.Authorization;
using Application.DTO.Product;
using Application.Interfaces.Services.Product;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Products;

public sealed class UpdateProductEndpoint : Endpoint<UpdateProductRequest, ProductDto>
{
    private readonly IProductService _service;

    public UpdateProductEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/api/products/{id:guid}");
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var dto = new MutationProductDto
        {
            Name = req.Name,
            SKU = req.SKU,
            Description = req.Description,
            UnitPrice = req.UnitPrice,
            CategoryId = req.CategoryId
        };

        var result = await _service.UpdateAsync(req.Id, dto, ct);
        if (result.IsSuccess)
            await SendOkAsync(result.Value!, ct);
        else
            await SendErrorsAsync(statusCode: 404, cancellation: ct);
    }
}
public sealed class UpdateProductRequest
{
    [FromRoute]
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double UnitPrice { get; set; }
    public Guid CategoryId { get; set; }
}
