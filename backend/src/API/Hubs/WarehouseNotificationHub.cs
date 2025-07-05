using Application.DTO.Warehouse;
using Application.Interfaces.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public sealed class WarehouseNotificationHub : Hub<IWarehouseNotificationClient>
{
    public async Task NotifyWarehouseAddedAsync(WarehouseDTO warehouse)
    {
        await Clients.All.NotifyWarehouseAddedAsync(warehouse);
    }
    
    public async Task NotifyWarehouseUpdatedAsync(WarehouseDTO warehouse)
    {
        await Clients.All.NotifyWarehouseUpdatedAsync(warehouse);
    }
    
    //todo: configure SignalR hub lifetime
    //todo: add unathorize attribute
    //todo: add delete notification
}