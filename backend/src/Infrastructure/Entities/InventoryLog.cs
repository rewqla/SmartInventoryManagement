namespace Infrastructure.Entities;

public sealed class InventoryLog : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int QuantityChanged { get; set; }
    public ChangeType  ChangeType { get; set; }
}