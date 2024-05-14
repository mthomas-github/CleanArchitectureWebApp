namespace ThirdPartyFreight.Web.Features.Approvals.Models;

public sealed class ApprovalResponse
{
    public Guid ApprovalId { get; init; }
    public long TaskId { get; init; }
    public Guid AgreementId { get; init; }
    public string Description { get; init; }
    public DateTime FirstApprovalStart { get; init; }
    public DateTime? FirstApprovalEnd { get; init; }
    public DateTime? SecondApprovalStart { get; init; }
    public DateTime? SecondApprovalEnd { get; init; }
    public DateTime? ThirdApprovalStart { get; init; }
    public DateTime? ThirdApprovalEnd { get; init; }
    public DateTime? CompletedOn { get; init; }
}
