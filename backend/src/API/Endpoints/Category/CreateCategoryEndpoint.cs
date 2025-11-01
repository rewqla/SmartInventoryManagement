using Application.DTO.Category;
using Application.Interfaces.Services.Category;
using FastEndpoints;

namespace API.Endpoints.Category;

public class CreateCategoryEndpoint : Endpoint<MutrateCategoryDTO, CategoryDTO>
{
    private readonly ICategoryService _service;

    public CreateCategoryEndpoint(ICategoryService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MutrateCategoryDTO req, CancellationToken ct)
    {
        var result = await _service.CreateAsync(req, ct);

        if (result.IsSuccess)
            await SendCreatedAtAsync<GetCategoryByIdEndpoint>(
                new { id = result.Value.Id },
                result.Value,
                generateAbsoluteUrl: true,
                cancellation: ct
            );
        else
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
    }
}
