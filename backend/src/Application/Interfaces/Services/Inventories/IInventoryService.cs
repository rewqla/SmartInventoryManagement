using Application.DTO.Inventory;
using Infrastructure.Entities;

namespace Application.Interfaces.Inventories;

public interface IInventoryService
{
    Task<IEnumerable<Inventory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryHistoryDTO>> GetHistoryAsync(Guid inventoryId, CancellationToken ct = default);
    Task<Inventory> CreateAsync(Inventory inventory, Guid changedById, CancellationToken cancellationToken = default);

    Task<Inventory?> UpdateAsync(Guid id, Inventory updatedInventory, Guid changedById,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, Guid changedById, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryDTO>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default);

    Task<Inventory?> ReleaseAsync(Guid inventoryId, int quantity, Guid changedById,
        CancellationToken cancellationToken = default);
}
