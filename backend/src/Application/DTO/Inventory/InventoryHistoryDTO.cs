using Infrastructure.Entities;

namespace Application.DTO.Inventory;

public sealed class InventoryHistoryDTO
{
    public Guid InventoryId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; } = null!;
    public int Quantity { get; set; }
    public string ChangeType { get; set; }
}
