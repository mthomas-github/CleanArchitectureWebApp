using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Abstractions.Hub;

public interface INotificationClient
{
    Task SendApprovalPayload(ApprovalResponse approval, CancellationToken cancellationToken = default);
    Task DeleteApprovalPayload(Guid id, CancellationToken cancellationToken = default);
}
