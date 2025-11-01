using Application.Interfaces.Services.Category;
using FastEndpoints;

namespace API.Endpoints.Category;

public sealed class SoftDeleteCategoryEndpoint : EndpointWithoutRequest
{
    private readonly ICategoryService _service;

    public SoftDeleteCategoryEndpoint(ICategoryService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Delete("/api/categories/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await _service.SoftDeleteAsync(id, ct);

        if (result.IsSuccess)
        {
            await SendNoContentAsync(ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}
