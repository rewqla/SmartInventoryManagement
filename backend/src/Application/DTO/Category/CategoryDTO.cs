namespace Application.DTO.Category;

public sealed class CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<CategoryProductDTO> Products { get; set; } = [];
}

public sealed class CategoryProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class MutrateCategoryDTO
{
    public string Name { get; set; } = string.Empty;
}
