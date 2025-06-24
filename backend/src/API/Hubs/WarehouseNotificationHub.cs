using Application.Interfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public sealed class WarehouseNotificationHub : Hub<IWarehouseNotificationClient>
{
    public async Task NotifyWarehouseAddedAsync(string warehouseName)
    {
        await Clients.All.NotifyWarehouseAddedAsync(warehouseName);
    }
}