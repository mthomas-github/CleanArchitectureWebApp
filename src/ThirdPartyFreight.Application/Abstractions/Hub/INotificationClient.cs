using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Abstractions.Hub;

public interface INotificationClient
{
    Task SendApprovalPayload(ApprovalResponse approval, CancellationToken cancellationToken = default);
    Task DeleteApprovalPayload(Guid workflowTaskId, CancellationToken cancellationToken = default);
    Task SendAgreementPayload(Guid agreementId, Status newStatus, CancellationToken cancellationToken = default);
    Task DeleteAgreementPayload(Guid agreementId, CancellationToken cancellationToken = default);
}
