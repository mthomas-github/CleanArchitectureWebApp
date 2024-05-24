using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Shared;

public sealed class ApprovalResponse
{
    public Guid ApprovalId { get; init; }
    public string TaskId { get; init; }
    public Guid AgreementId { get; init; }
    public AgreementType AgreementType { get; init; }
    public Guid WorkFlowTaskId { get; init; }
    public ApproverType Approver { get; init; }
    public DateTime? FirstApprovalStart { get; init; }
    public DateTime? FirstApprovalEnd { get; init; }
    public DateTime? SecondApprovalStart { get; init; }
    public DateTime? SecondApprovalEnd { get; init; }
    public DateTime? ThirdApprovalStart { get; init; }
    public DateTime? ThirdApprovalEnd { get; init; }
    public DateTime? CompletedOn { get; init; }
    public bool? Voided { get;init; }
}
