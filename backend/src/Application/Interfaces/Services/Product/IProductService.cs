using Application.Common;

namespace Application.Interfaces.Services.Product;

public interface IProductService
{
    Task<Result<byte[]>> GenerateProductReportAsync(CancellationToken cancellationToken = default);
}