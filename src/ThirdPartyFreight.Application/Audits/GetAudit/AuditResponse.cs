namespace ThirdPartyFreight.Application.Audits.GetAudit;

public sealed class AuditResponse
{
    public Guid Id { get; init; }
    public Guid AgreementId { get; init; }
    public DateOnly AuditDateUtc { get; init; }
    public bool IsAuditActive { get;init; }
    public DateOnly AuditCompleteDateUtc { get; init; }

}
