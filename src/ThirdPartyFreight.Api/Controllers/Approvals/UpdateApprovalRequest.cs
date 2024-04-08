using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Api.Controllers.Approvals;

public record UpdateApprovalRequest(Guid ApprovalId, Approval Approval);
