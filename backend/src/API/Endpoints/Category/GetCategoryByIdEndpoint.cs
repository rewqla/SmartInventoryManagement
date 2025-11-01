using Application.DTO.Category;
using Application.Interfaces.Services.Category;
using FastEndpoints;

namespace API.Endpoints.Category;

public class GetCategoryByIdEndpoint : Endpoint<GetCategoryByIdRequest, CategoryDTO>
{
    private readonly ICategoryService _service;

    public GetCategoryByIdEndpoint(ICategoryService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/categories/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCategoryByIdRequest req, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(req.Id, ct);

        if (result.IsSuccess)
            await SendOkAsync(result.Value!, ct);
        else
            await SendNotFoundAsync(ct);
    }
}

public class GetCategoryByIdRequest
{
    public Guid Id { get; set; }
}
