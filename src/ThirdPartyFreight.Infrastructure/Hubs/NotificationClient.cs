using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Infrastructure.Hubs;

internal sealed class NotificationClient(IHubContext<NotificationHub, INotificationClient> hubContext, ILogger<NotificationClient> logger) : INotificationClient
{
    public async Task SendApprovalPayload(ApprovalResponse approval, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing SendApprovalPayload with {Approval}", approval.ApprovalId);
        await hubContext.Clients.All.SendApprovalPayload(approval, cancellationToken);
    }

    public async Task DeleteApprovalPayload(Guid workflowTaskId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing DeleteApprovalPayload");
        await hubContext.Clients.All.DeleteApprovalPayload(workflowTaskId, cancellationToken);
    }

    public async Task SendAgreementPayload(Guid agreementId, Status newStatus, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing SendAgreementPayload with {Agreement}", agreementId);
        await hubContext.Clients.All.SendAgreementPayload(agreementId, newStatus, cancellationToken);
    }

    public async Task DeleteAgreementPayload(Guid agreementId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing DeleteAgreementPayload");
        await hubContext.Clients.All.DeleteAgreementPayload(agreementId, cancellationToken);
    }
}
