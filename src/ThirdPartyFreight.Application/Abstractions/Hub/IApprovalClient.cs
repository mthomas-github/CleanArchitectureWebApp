using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Abstractions.Hub;

public interface IApprovalClient
{
    Task SendPayload(ApprovalResponse approval, CancellationToken cancellationToken = default);
    Task DeletePayload(Guid id, CancellationToken cancellationToken = default);
    
}
