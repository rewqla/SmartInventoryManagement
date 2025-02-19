using Application.Common;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Infrastructure.Data;
using Infrastructure.Interfaces.Repositories;

namespace Application.Services.Product;

public class ProductService : IProductService
{
    private readonly IReportService<Infrastructure.Entities.Product> _productReportService;
    private readonly IProductRepository _productRepository;

    public ProductService(IReportService<Infrastructure.Entities.Product> productReportService, IProductRepository productRepository)
    {
        _productReportService = productReportService;
        _productRepository = productRepository;
    }

    public async Task<Result<byte[]>> GenerateProductReportAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var report = _productReportService.GenerateReport(products);

        return Result<byte[]>.Success(report);
    }
}