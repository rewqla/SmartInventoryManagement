using API.Authorization;
using Application.Interfaces.Services.Product;
using FastEndpoints;

namespace API.Endpoints.Products;

public sealed class SoftDeleteProductEndpoint : EndpointWithoutRequest
{
    private readonly IProductService _service;

    public SoftDeleteProductEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Delete("/api/products/{id:guid}");
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await _service.SoftDeleteAsync(id, ct);

        if (result.IsSuccess)
            await SendOkAsync(true, ct);
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
