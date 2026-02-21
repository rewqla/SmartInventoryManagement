using Application.DTO.Category;
using Application.Interfaces.Services.Category;
using Infrastructure.Interfaces.Repositories;
using SharedKernel.Exceptions;
using SharedKernel.ResultPattern;

namespace Application.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<CategoryDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);
        var dto = categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
        });

        return Result<IEnumerable<CategoryDTO>>.Success(dto);
    }

    public async Task<Result<CategoryDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var c = await _repository.FindByIdAsync(id, cancellationToken);
        if (c is null)
            return Result<CategoryDTO>.Failure(CommonErrors.NotFoundById("category", id));

        var dto = new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Products = c.Products.Select(p => new CategoryProductDTO { Id = p.Id, Name = p.Name }).ToList()
        };

        return Result<CategoryDTO>.Success(dto);
    }

    public async Task<Result<CategoryDTO>> CreateAsync(MutrateCategoryDTO createCategory,
        CancellationToken cancellationToken = default)
    {
        var category = new Infrastructure.Entities.Category { Name = createCategory.Name };
        await _repository.AddAsync(category, cancellationToken);
        await _repository.CompleteAsync();

        return Result<CategoryDTO>.Success(new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Products = new List<CategoryProductDTO>()
        });
    }

    public async Task<Result<CategoryDTO>> UpdateAsync(Guid id, MutrateCategoryDTO updatedCategory,
        CancellationToken cancellationToken = default)
    {
        var category = await _repository.FindByIdAsync(id, cancellationToken);
        if (category is null)
            return Result<CategoryDTO>.Failure(CommonErrors.NotFoundById("category", id));

        category.Name = updatedCategory.Name;
        _repository.Update(category);
        await _repository.CompleteAsync();

        var dto = new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Products = category.Products.Select(p => new CategoryProductDTO { Id = p.Id, Name = p.Name }).ToList()
        };

        return Result<CategoryDTO>.Success(dto);
    }

    public async Task<Result<bool>> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetCategoryWithProductsAsync(id, cancellationToken);

        if (category is null || category.Products.Any())
            return Result<bool>.Failure(CommonErrors.NotFoundById("category", id));

        _repository.Delete(category);
        await _repository.CompleteAsync();
        return Result<bool>.Success(true);
    }
}
