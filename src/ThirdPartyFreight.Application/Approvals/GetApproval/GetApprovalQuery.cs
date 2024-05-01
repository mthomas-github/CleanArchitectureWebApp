using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Approvals.GetApproval;

public record GetApprovalQuery(Guid ApprovalId) : IQuery<ApprovalResponse>;
