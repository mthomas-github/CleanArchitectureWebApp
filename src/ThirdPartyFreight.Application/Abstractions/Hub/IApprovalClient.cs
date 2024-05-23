using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Abstractions.Hub;

public interface IApprovalClient
{
    Task SendPayload(Approval approval, CancellationToken cancellationToken = default);
}
