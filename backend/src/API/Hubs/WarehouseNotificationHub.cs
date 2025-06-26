using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public sealed class WarehouseNotificationHub : Hub<IWarehouseNotificationClient>
{
    public async Task NotifyWarehouseAddedAsync(WarehouseDTO warehouse)
    {
        await Clients.All.NotifyWarehouseAddedAsync(warehouse);
    }
}