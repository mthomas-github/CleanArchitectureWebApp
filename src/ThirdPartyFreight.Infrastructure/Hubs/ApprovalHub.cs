using Microsoft.AspNetCore.SignalR;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Infrastructure.Hubs;

public sealed class ApprovalHub : Hub<IApprovalClient>
{
    public async Task SendApprovalPayload(Approval approval)
        => await Clients.Client(Context.ConnectionId).SendPayload(approval);
}
