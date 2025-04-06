using Application.Common;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Infrastructure.Data;
using Infrastructure.Interfaces.Repositories;

namespace Application.Services.Product;

public class ProductService : IProductService
{
    private readonly IReportService _reportService;
    private readonly IProductRepository _productRepository;

    public ProductService(IReportService reportService, IProductRepository productRepository)
    {
        _reportService = reportService;
        _productRepository = productRepository;
    }

    public async Task<Result<byte[]>> GenerateProductReportAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var report = _reportService.GenerateReport(products);

        return Result<byte[]>.Success(report);
    }
}