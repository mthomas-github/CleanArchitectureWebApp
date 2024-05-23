using Microsoft.AspNetCore.SignalR;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Infrastructure.Hubs;

internal sealed class ApprovalClient(IHubContext<ApprovalHub, IApprovalClient> hubContext) : IApprovalClient
{
    public async Task SendPayload(Approval approval, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients.All.SendPayload(approval, cancellationToken);
    }
}
