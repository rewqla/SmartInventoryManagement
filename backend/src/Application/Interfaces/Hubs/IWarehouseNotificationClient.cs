using Application.DTO.Warehouse;

namespace Application.Interfaces.Hubs;


public interface IWarehouseNotificationClient
{
    Task NotifyWarehouseAddedAsync(WarehouseDTO warehouse);
    Task NotifyWarehouseUpdatedAsync(WarehouseDTO warehouse);
}