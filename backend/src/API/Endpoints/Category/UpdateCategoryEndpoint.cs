using Application.DTO.Category;
using Application.Interfaces.Services.Category;
using FastEndpoints;

namespace API.Endpoints.Category;

public sealed class UpdateCategoryEndpoint : Endpoint<MutrateCategoryDTO, CategoryDTO>
{
    private readonly ICategoryService _service;

    public UpdateCategoryEndpoint(ICategoryService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/api/categories/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MutrateCategoryDTO req, CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await _service.UpdateAsync(id, req, ct);

        if (result.IsSuccess && result.Value is not null)
            await SendOkAsync(result.Value, ct);
        else
            await SendNotFoundAsync(ct);
    }
}
