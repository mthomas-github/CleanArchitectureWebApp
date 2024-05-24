using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Infrastructure.Hubs;

internal sealed class ApprovalClient(IHubContext<ApprovalHub, IApprovalClient> hubContext, ILogger<ApprovalClient> logger) : IApprovalClient
{
    public async Task SendPayload(ApprovalResponse approval, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing SendPayload with {Approval}", approval.ApprovalId);
        await hubContext.Clients.All.SendPayload(approval, cancellationToken);
    }

    public Task DeletePayload(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
