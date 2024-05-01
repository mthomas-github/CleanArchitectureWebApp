namespace ThirdPartyFreight.Application.Shared;

public sealed class ApprovalResponse
{
    public Guid AgreementId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? FirstApprovalOnUtc { get; private set; }
    public DateTime? FirstApprovalEndUtc { get; private set; }
    public DateTime? SecondApprovalOnUtc { get; private set; }
    public DateTime? SecondApprovalEndUtc { get; private set; }
    public DateTime? ThirdApprovalOnUtc { get; private set; }
    public DateTime? ThirdApprovalEndUtc { get; private set; }
    public DateTime? CompletedOn { get; private set; }
}
