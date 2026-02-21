using Application.DTO.Category;
using SharedKernel.ResultPattern;

namespace Application.Interfaces.Services.Category;

public interface ICategoryService
{
    Task<Result<IEnumerable<CategoryDTO>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<CategoryDTO>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CategoryDTO>> CreateAsync(MutrateCategoryDTO createCategory, CancellationToken cancellationToken = default);
    Task<Result<CategoryDTO>> UpdateAsync(Guid id, MutrateCategoryDTO updatedCategory, CancellationToken cancellationToken = default);
    Task<Result<bool>> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
