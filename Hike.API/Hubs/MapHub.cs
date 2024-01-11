using Hike.UseCases.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.SignalR;

namespace Hike.API.Hubs;

public class MapHub : Hub
{
    public async Task SendLocation(string userName, double latitude, double longitude)
    {
        Console.WriteLine(Context.UserIdentifier);

        if (Context.UserIdentifier != null)
        {
            await Clients.All.SendAsync("ReceiveLocation", Context.UserIdentifier, userName, latitude, longitude);
        }
    }
}