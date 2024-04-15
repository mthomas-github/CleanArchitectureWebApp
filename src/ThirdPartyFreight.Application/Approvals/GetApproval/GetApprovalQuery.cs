using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Approvals.GetApproval;

public record GetApprovalQuery(Guid ApprovalId) : IQuery<Approval>;
