using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals.Events;

namespace ThirdPartyFreight.Domain.Approvals;

public sealed class Approval : Entity
{
    private Approval(
        Guid id,
        Guid agreementId,
        DateTime createdOnUtc,
        DateTime? firstApprovalOnUtc,
        DateTime? firstApprovalEndUtc,
        DateTime? secondApprovalOnUtc,
        DateTime? secondApprovalEndUtc,
        DateTime? thirdApprovalOnUtc,
        DateTime? thirdApprovalEndUtc,
        DateTime? completedOn)
        : base(id)
    {
        AgreementId = agreementId;
        CreatedOnUtc = createdOnUtc;
        FirstApprovalOnUtc = firstApprovalOnUtc;
        FirstApprovalEndUtc = firstApprovalEndUtc;
        SecondApprovalOnUtc = secondApprovalOnUtc;
        SecondApprovalEndUtc = secondApprovalEndUtc;
        ThirdApprovalOnUtc = thirdApprovalOnUtc;
        ThirdApprovalEndUtc = thirdApprovalEndUtc;
        CompletedOn = completedOn;
    }

    private Approval() { }
    public Guid AgreementId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? FirstApprovalOnUtc { get; private set; }
    public DateTime? FirstApprovalEndUtc { get; private set; }
    public DateTime? SecondApprovalOnUtc { get; private set; }
    public DateTime? SecondApprovalEndUtc { get; private set; }
    public DateTime? ThirdApprovalOnUtc { get; private set; }
    public DateTime? ThirdApprovalEndUtc { get; private set; }
    public DateTime? CompletedOn { get; private set; }

    public static Approval Create(
        Guid agreementId,
        DateTime createdOnUtc)
    {
        var approval = new Approval(
            Guid.NewGuid(),
                agreementId,
                createdOnUtc,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        approval.RaiseDomainEvent(new ApprovalCreatedDomainEvent(approval.Id));
        return approval;
    }

    public static void Update(
        Approval approval,
        DateTime? firstApprovalOnUtc,
        DateTime? firstApprovalEndUtc,
        DateTime? secondApprovalOnUtc,
        DateTime? secondApprovalEndUtc,
        DateTime? thirdApprovalOnUtc,
        DateTime? thirdApprovalEndUtc,
        DateTime? completedOn)
    {
        approval.FirstApprovalOnUtc = firstApprovalOnUtc;
        approval.FirstApprovalEndUtc = firstApprovalEndUtc;
        approval.SecondApprovalOnUtc = secondApprovalOnUtc;
        approval.SecondApprovalEndUtc = secondApprovalEndUtc;
        approval.ThirdApprovalOnUtc = thirdApprovalOnUtc;
        approval.ThirdApprovalEndUtc = thirdApprovalEndUtc;
        approval.CompletedOn = completedOn;

        approval.RaiseDomainEvent(new ApprovalUpdatedDomainEvent(approval.Id));
    }
}
