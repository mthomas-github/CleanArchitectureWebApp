using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Approvals.GetApproval;

public record GetApprovalQuery(Guid ApprovalId) : IQuery<ApprovalResponse>;
