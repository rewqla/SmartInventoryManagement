using Application.Common;
using Application.DTO.Product;
using Application.Exceptions;
using Application.Interfaces.Services.Product;
using Application.Interfaces.Services.Report;
using Infrastructure.Data;
using Infrastructure.Interfaces.Repositories;

namespace Application.Services.Product;

public class ProductService : IProductService
{
    private readonly IReportService _reportService;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public ProductService(IReportService reportService, IProductRepository productRepository,
        IInventoryRepository inventoryRepository, ICategoryRepository categoryRepository)
    {
        _reportService = reportService;
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
        _categoryRepository = categoryRepository;
    }

    private static ProductDto MapToDto(Infrastructure.Entities.Product product)
        => new()
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };

    public async Task<Result<byte[]>> GenerateProductReportAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var report = _reportService.GenerateReport(products);

        return Result<byte[]>.Success(report);
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllWithCategoriesAsync(cancellationToken);

        var dto = products.Select(MapToDto);

        return Result<IEnumerable<ProductDto>>.Success(dto);
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.FindByIdWithCategoryAsync(id, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure(CommonErrors.NotFoundById("product", id));

        return Result<ProductDto>.Success(MapToDto(product));
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetByCategoryWithCategoryAsync(categoryId, cancellationToken);
        return Result<IEnumerable<ProductDto>>.Success(products.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<ShortProductDto>>> FindInWarehousesAsync(Guid warehouseId,
        CancellationToken cancellationToken = default)
    {
        var inventories = await _inventoryRepository.GetByWarehouseAsync(warehouseId,cancellationToken);
        var products = inventories
            .Select(x => x.Product)
            .Distinct()
            .ToList();

        if (products.Count == 0)
            return Result<IEnumerable<ShortProductDto>>.Failure(CommonErrors.NotFoundById("warehouse", warehouseId));

        var dto = products.Select(p => new ShortProductDto
        {
            Id = p.Id,
            Name = p.Name,
            SKU = p.SKU,
            UnitPrice = p.UnitPrice
        });

        return Result<IEnumerable<ShortProductDto>>.Success(dto);
    }

    public async Task<Result<ProductDto>> CreateAsync(MutationProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = new Infrastructure.Entities.Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Description = dto.Description,
            UnitPrice = dto.UnitPrice,
            CategoryId = dto.CategoryId
        };

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.CompleteAsync();

        return Result<ProductDto>.Success(MapToDto(product));
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, MutationProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.FindByIdWithCategoryAsync(id, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure(CommonErrors.NotFound("product"));

        product.Name = dto.Name;
        product.SKU = dto.SKU;
        product.Description = dto.Description;
        product.UnitPrice = dto.UnitPrice;

        _productRepository.Update(product);
        await _productRepository.CompleteAsync();

        return Result<ProductDto>.Success(MapToDto(product));
    }

    public async Task<Result<bool>> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.FindByIdAsync(id, cancellationToken);
        if (product is null)
            return Result<bool>.Failure(CommonErrors.NotFoundById("product", id));

        var hasInventory = (await _inventoryRepository
                .GetAllAsync(cancellationToken))
            .Any(x => x.ProductId == id);

        if (hasInventory)
            return Result<bool>.Failure(new Error("Product.CannotBeDeleted", "This product is used in inventories."));

        _productRepository.Delete(product);
        await _productRepository.CompleteAsync();

        return Result<bool>.Success(true);
    }
}
