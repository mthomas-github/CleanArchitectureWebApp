using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Abstractions.Hub;

public interface INotificationClient
{
    Task SendApprovalPayload(ApprovalResponse approval, CancellationToken cancellationToken = default);
    Task DeleteApprovalPayload(Guid workflowTaskid, CancellationToken cancellationToken = default);
    Task SendAgreementPayload(AgreementResponse agreement, CancellationToken cancellationToken = default);
    Task DeleteAgreementPayload(Guid agreementId, CancellationToken cancellationToken = default);
}
