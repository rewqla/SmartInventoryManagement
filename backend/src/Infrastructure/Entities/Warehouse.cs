namespace Infrastructure.Entities;

public sealed class Warehouse : BaseEntity
{
    public string Name { get; set; }
    public string Location { get; set; }
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}