namespace Application.DTO.Product;

public sealed class ShortProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public double UnitPrice { get; set; }
}
