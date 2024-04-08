using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Approvals.UpdateApproval;

public sealed record UpdateApprovalCommand(
    Guid ApprovalId,
    Approval Approval) : ICommand;
