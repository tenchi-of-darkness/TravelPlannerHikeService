using Microsoft.AspNetCore.SignalR;

namespace Hike.API.Hubs;

public class MapHub : Hub
{
    public async Task SendLocation(string userId, string userName, double latitude, double longitude)
    {
        await Clients.All.SendAsync("Receive Location", userId, userName, latitude, longitude);
    }
}