using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Audits.Events;

namespace ThirdPartyFreight.Domain.Audits;

public class Audit : Entity
{
    private Audit(
        Guid id,
        Guid agreementId,
        AuditInfo auditInfo)
        : base(id)
    {
        AgreementId = agreementId;
        AuditInfo = auditInfo;
    }

    private Audit()
    {
        // Required by EF
    }
    public Guid AgreementId { get; private set; }

    public AuditInfo AuditInfo { get; private set; }


    public static Audit Create(
        Guid agreementId,
        AuditInfo auditInfo)
    {
        var audit = new Audit(
                       Guid.NewGuid(),
                                  agreementId,
                                  auditInfo);

        audit.RaiseDomainEvent(new AuditCreatedDomainEvent(audit.Id));
        return audit;
    }


}
