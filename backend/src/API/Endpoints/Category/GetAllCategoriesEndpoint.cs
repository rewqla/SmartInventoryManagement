using API.Authorization;
using Application.DTO.Category;
using Application.Interfaces.Services.Category;
using FastEndpoints;

namespace API.Endpoints.Category;

public class GetAllCategoriesEndpoint : EndpointWithoutRequest<IEnumerable<CategoryDTO>>
{
    private readonly ICategoryService _service;

    public GetAllCategoriesEndpoint(ICategoryService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/api/categories");
        Roles(PolicyRoles.Admin, PolicyRoles.Manager);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);

        if (result.IsSuccess)
            await SendOkAsync(result.Value!, ct);
        else
            await SendErrorsAsync(cancellation: ct);
    }
}
