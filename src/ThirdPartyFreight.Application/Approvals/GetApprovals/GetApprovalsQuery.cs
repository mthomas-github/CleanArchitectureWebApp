using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Approvals.GetApprovals;

public record GetApprovalsQuery() : IQuery<IReadOnlyList<ApprovalResponse>>;
