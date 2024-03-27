using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

public sealed record AddApprovalCommand(Guid AgreementId) : ICommand;
