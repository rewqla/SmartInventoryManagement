using API.Authorization;
using Application.DTO.Product;
using Application.Interfaces.Services.Product;
using FastEndpoints;

namespace API.Endpoints.Products;

public sealed class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, ProductDto>
{
    private readonly IProductService _service;

    public GetProductByIdEndpoint(IProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/products/{id:guid}");
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(GetProductByIdRequest request, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(request.Id, ct);

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

public class GetProductByIdRequest
{
    public Guid Id { get; set; }
}
