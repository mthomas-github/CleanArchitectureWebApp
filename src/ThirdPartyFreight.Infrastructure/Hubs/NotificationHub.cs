using Microsoft.AspNetCore.SignalR;
using ThirdPartyFreight.Application.Abstractions.Hub;

namespace ThirdPartyFreight.Infrastructure.Hubs;

public sealed class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ReceiveNotification($"Thank you for connecting...");

        await base.OnConnectedAsync();
    }
}
