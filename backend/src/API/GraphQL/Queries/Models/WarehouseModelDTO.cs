namespace API.GraphQL.Queries.Models;

public class WarehouseModelDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IEnumerable<InventoryModelDTO> Inventories { get; set; } = new List<InventoryModelDTO>();
}