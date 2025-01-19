using Application.Common;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Infrastructure.Data;

namespace Application.Services.Product;

public class ProductService : IProductService
{
    private readonly IReportService<Infrastructure.Entities.Product> _productReportService;
    private readonly InventoryContext _context;

    public ProductService(IReportService<Infrastructure.Entities.Product> productReportService, InventoryContext context)
    {
        _productReportService = productReportService;
        _context = context;
    }

    public async Task<Result<byte[]>> GenerateProductReportAsync(CancellationToken cancellationToken = default)
    {
        var products = _context.Products.ToList();
        var report = _productReportService.GenerateReport(products);

        return Result<byte[]>.Success(report);
    }
}