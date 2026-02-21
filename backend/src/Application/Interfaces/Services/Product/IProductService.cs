using Application.DTO.Product;
using SharedKernel.ResultPattern;

namespace Application.Interfaces.Services.Product;

public interface IProductService
{
    Task<Result<byte[]>> GenerateProductReportAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProductDto>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ShortProductDto>>> FindInWarehousesAsync(Guid warehouseId, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> CreateAsync(MutationProductDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> UpdateAsync(Guid id, MutationProductDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
