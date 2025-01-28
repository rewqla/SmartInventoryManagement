namespace API.GraphQL.Queries.Models;

public class InventoryModelDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}