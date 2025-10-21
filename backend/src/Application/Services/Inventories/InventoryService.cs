using Application.DTO.Inventory;
using Application.Interfaces.Inventories;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Inventories;

public sealed class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _inventoryRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryHistoryDTO>> GetHistoryAsync(Guid inventoryId, CancellationToken ct = default)
    {
        var inventory = await _inventoryRepository.GetWithLogsAsync(inventoryId, ct);

        if (inventory == null) return [];

        return inventory.InventoryLogs
            .Select(log => new InventoryHistoryDTO
            {
                InventoryId = inventory.Id,
                ChangedAt = log.Timestamp,
                ChangedBy = log.User.FullName,
                Quantity = log.QuantityChanged,
                ChangeType = log.ChangeType.ToString()
            })
            .OrderByDescending(log => log.ChangedAt);
    }

    public async Task<Inventory> CreateAsync(Inventory inventory, Guid changedById,
        CancellationToken cancellationToken = default)
    {
        var created = await _inventoryRepository.AddAsync(inventory, cancellationToken);

        created.InventoryLogs.Add(new InventoryLog
        {
            Inventory = created,
            InventoryId = created.Id,
            Timestamp = DateTime.UtcNow,
            QuantityChanged = created.Quantity,
            ChangeType = ChangeType.Added,
            ChangedById = changedById
        });

        await _inventoryRepository.CompleteAsync();
        return created;
    }

    public async Task<Inventory?> UpdateAsync(Guid id, Inventory updatedInventory, Guid changedById,
        CancellationToken cancellationToken = default)
    {
        var existing = await _inventoryRepository.FindByIdAsync(id, cancellationToken);
        if (existing is null)
            return null;

        var quantityChange = updatedInventory.Quantity - existing.Quantity;
        existing.Quantity = updatedInventory.Quantity;

        if (quantityChange != 0)
        {
            existing.InventoryLogs.Add(new InventoryLog
            {
                Inventory = existing,
                InventoryId = existing.Id,
                Timestamp = DateTime.UtcNow,
                QuantityChanged = quantityChange,
                ChangeType = ChangeType.Adjusted,
                ChangedById = changedById
            });
        }

        _inventoryRepository.Update(existing);
        await _inventoryRepository.CompleteAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid changedById, CancellationToken cancellationToken = default)
    {
        var existing = await _inventoryRepository.FindByIdAsync(id, cancellationToken);
        if (existing is null)
            return false;

        existing.InventoryLogs.Add(new InventoryLog
        {
            Inventory = existing,
            InventoryId = existing.Id,
            Timestamp = DateTime.UtcNow,
            QuantityChanged = -existing.Quantity,
            ChangeType = ChangeType.Removed,
            ChangedById = changedById
        });

        _inventoryRepository.Delete(existing);
        await _inventoryRepository.CompleteAsync();

        return true;
    }

    public async Task<IEnumerable<InventoryDTO>> GetByWarehouseAsync(Guid warehouseId,
        CancellationToken cancellationToken = default)
    {
        var inventories = await _inventoryRepository.GetByWarehouseAsync(warehouseId, cancellationToken);

        return inventories.Select(i => new InventoryDTO
        {
            Id = i.Id,
            ProductName = i.Product.Name,
            WarehouseName = i.Warehouse.Name,
            Quantity = i.Quantity
        });
    }

    public async Task<Inventory?> ReleaseAsync(Guid inventoryId, int quantity, Guid changedById,
        CancellationToken cancellationToken = default)
    {
        var inventory = await _inventoryRepository.FindByIdAsync(inventoryId, cancellationToken);
        if (inventory is null || quantity <= 0 || inventory.Quantity < quantity)
            return null;

        inventory.Quantity -= quantity;

        inventory.InventoryLogs.Add(new InventoryLog
        {
            Inventory = inventory,
            InventoryId = inventory.Id,
            Timestamp = DateTime.UtcNow,
            QuantityChanged = -quantity,
            ChangeType = ChangeType.Released,
            ChangedById = changedById
        });

        _inventoryRepository.Update(inventory);
        await _inventoryRepository.CompleteAsync();

        return inventory;
    }
}
