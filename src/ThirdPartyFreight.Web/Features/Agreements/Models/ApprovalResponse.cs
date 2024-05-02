namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class ApprovalResponse
{
    public Guid AgreementId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? FirstApprovalOnUtc { get; set; }
    public DateTime? FirstApprovalEndUtc { get; set; }
    public DateTime? SecondApprovalOnUtc { get; set; }
    public DateTime? SecondApprovalEndUtc { get; set; }
    public DateTime? ThirdApprovalOnUtc { get; set; }
    public DateTime? ThirdApprovalEndUtc { get; set; }
    public DateTime? CompletedOn { get; set; }
}
