using Microsoft.AspNetCore.SignalR;
using ThirdPartyFreight.Application.Abstractions.Hub;

namespace ThirdPartyFreight.Infrastructure.Hubs;

public class NotificationHub : Hub<INotificationClient>
{
    
}
