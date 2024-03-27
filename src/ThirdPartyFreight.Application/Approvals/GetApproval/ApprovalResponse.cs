namespace ThirdPartyFreight.Application.Approvals.GetApproval;

public sealed class ApprovalResponse
{
    public Guid Id { get; init; }
    public Guid AgreementId { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? FirstApprovalOnUtc { get; init; }
    public DateTime? FirstApprovalEndUtc { get; init; }
    public DateTime? SecondApprovalOnUtc { get; init; }
    public DateTime? SecondApprovalEndUtc { get; init; }
    public DateTime? ThirdApprovalOnUtc { get; init; }
    public DateTime? ThirdApprovalEndUtc { get; init; }
    public DateTime? CompletedOn { get; init; }

}
