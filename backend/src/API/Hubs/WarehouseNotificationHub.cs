using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public sealed class WarehouseNotificationHub : Hub
{
    // public async Task NotifyWarehouseAddedAsync(string warehouseName)
    // {
    //     await Clients.All.SendAsync("WarehouseAdded", warehouseName);
    // }
}