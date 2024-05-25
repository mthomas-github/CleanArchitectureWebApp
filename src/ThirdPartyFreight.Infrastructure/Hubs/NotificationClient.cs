using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Infrastructure.Hubs;

internal sealed class NotificationClient(IHubContext<NotificationHub, INotificationClient> hubContext, ILogger<NotificationClient> logger) : INotificationClient
{
    public async Task SendApprovalPayload(ApprovalResponse approval, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing SendPayload with {Approval}", approval.ApprovalId);
        await hubContext.Clients.All.SendApprovalPayload(approval, cancellationToken);
    }

    public async Task DeleteApprovalPayload(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing DeletePayload");
        await hubContext.Clients.All.DeleteApprovalPayload(id, cancellationToken);
    }
}
