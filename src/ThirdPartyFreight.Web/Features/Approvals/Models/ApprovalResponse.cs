using System.ComponentModel;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Web.Extensions;

namespace ThirdPartyFreight.Web.Features.Approvals.Models;

internal sealed class ApprovalResponse
{
    public Guid ApprovalId { get; init; }
    public string TaskId { get; init; }
    public Guid AgreementId { get; init; }
    public AgreementType AgreementType { get; init; }
    public Guid WorkFlowTaskId { get; init; }
    public ApproverType Approver { get; init; }
    public DateTime FirstApprovalStart { get; init; }
    public DateTime? FirstApprovalEnd { get; init; }
    public DateTime? SecondApprovalStart { get; init; }
    public DateTime? SecondApprovalEnd { get; init; }
    public DateTime? ThirdApprovalStart { get; init; }
    public DateTime? ThirdApprovalEnd { get; init; }
    public DateTime? CompletedOn { get; init; }
    public string ProcessId { get; init; }
    
    public string ApproverDisplayName => GetEnumDisplayName.GetDisplayName(Approver);

    
}
public enum ApproverType
{
    [Description("TPF Team")]
    TpfTeam,
    [Description("TMS Team")]
    TmsTeam,
    [Description("MDM Team")]
    MdmTeam,
}
