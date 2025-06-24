namespace Application.Interfaces.Hubs;


public interface IWarehouseNotificationClient
{
    Task NotifyWarehouseAddedAsync(string warehouseName);
}