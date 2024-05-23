using Microsoft.AspNetCore.SignalR;

namespace ThirdPartyFreight.Infrastructure.Hubs;

public sealed class ApprovalHub : Hub
{
    public Task SendUpdates()
    {
        return Task.CompletedTask;
    }
}
