using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals.Events;

namespace ThirdPartyFreight.Domain.Approvals;

public sealed class Approval : Entity
{
    private Approval(
        Guid id,
        Guid agreementId,
        string? taskId,
        DateTime createdOnUtc,
        DateTime? firstApprovalOnUtc,
        DateTime? firstApprovalEndUtc,
        DateTime? secondApprovalOnUtc,
        DateTime? secondApprovalEndUtc,
        DateTime? thirdApprovalOnUtc,
        DateTime? thirdApprovalEndUtc,
        DateTime? completedOn,
        bool? voided)
        : base(id)
    {
        AgreementId = agreementId;
        TaskId = taskId;
        CreatedOnUtc = createdOnUtc;
        FirstApprovalOnUtc = firstApprovalOnUtc;
        FirstApprovalEndUtc = firstApprovalEndUtc;
        SecondApprovalOnUtc = secondApprovalOnUtc;
        SecondApprovalEndUtc = secondApprovalEndUtc;
        ThirdApprovalOnUtc = thirdApprovalOnUtc;
        ThirdApprovalEndUtc = thirdApprovalEndUtc;
        CompletedOn = completedOn;
        Voided = voided;
    }

    private Approval() { }
    public Guid AgreementId { get; private set; }
    public string? TaskId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? FirstApprovalOnUtc { get; private set; }
    public DateTime? FirstApprovalEndUtc { get; private set; }
    public DateTime? SecondApprovalOnUtc { get; private set; }
    public DateTime? SecondApprovalEndUtc { get; private set; }
    public DateTime? ThirdApprovalOnUtc { get; private set; }
    public DateTime? ThirdApprovalEndUtc { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public bool? Voided { get; private set; }

    public static Approval Create(
        Guid agreementId,
        DateTime createdOnUtc
        )
    {
        var approval = new Approval(
            Guid.NewGuid(),
                agreementId,
                null,
                createdOnUtc,
                null,
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

    public Result SetUpdatedValues(
        string? taskId,
        DateTime? firstApprovalOnUtc,
        DateTime? firstApprovalEndUtc,
        DateTime? secondApprovalOnUtc,
        DateTime? secondApprovalEndUtc,
        DateTime? thirdApprovalOnUtc,
        DateTime? thirdApprovalEndUtc,
        DateTime? completedOn,
        bool? voided)

    {
        TaskId = taskId;
        FirstApprovalOnUtc = firstApprovalOnUtc;
        FirstApprovalEndUtc = firstApprovalEndUtc;
        SecondApprovalOnUtc = secondApprovalOnUtc;
        SecondApprovalEndUtc = secondApprovalEndUtc;
        ThirdApprovalOnUtc = thirdApprovalOnUtc;
        ThirdApprovalEndUtc = thirdApprovalEndUtc;
        CompletedOn = completedOn;
        Voided = voided;
        
        RaiseDomainEvent(new ApprovalUpdatedDomainEvent(Id));

        return Result.Success();
    }
}
