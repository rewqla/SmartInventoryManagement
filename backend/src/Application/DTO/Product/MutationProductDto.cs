namespace Application.DTO.Product;

public class MutationProductDto
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double UnitPrice { get; set; }
    public Guid CategoryId { get; set; }
}
