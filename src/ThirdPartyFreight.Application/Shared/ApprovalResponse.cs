using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Shared;

public sealed class ApprovalResponse
{
    public Guid ApprovalId { get; private set; }
    public string TaskId { get; private set; }
    public Guid AgreementId { get; private set; }
    public AgreementType AgreementType { get; private set; }
    public Guid WorkFlowTaskId { get; private set; }
    public ApproverType Approver { get; private set; }
    public DateTime? FirstApprovalStart { get; private set; }
    public DateTime? FirstApprovalEnd { get; private set; }
    public DateTime? SecondApprovalStart { get; private set; }
    public DateTime? SecondApprovalEnd { get; private set; }
    public DateTime? ThirdApprovalStart { get; private set; }
    public DateTime? ThirdApprovalEnd { get; private set; }
    public DateTime? CompletedOn { get; private set; }
}
