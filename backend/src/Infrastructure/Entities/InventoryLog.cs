namespace Infrastructure.Entities;

public sealed class InventoryLog : BaseEntity
{
    public Guid InventoryId { get; set; }
    public Inventory Inventory { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int QuantityChanged { get; set; }
    public ChangeType  ChangeType { get; set; }
    public Guid ChangedById { get; set; }
    public User User { get; set; } = null!;
}
