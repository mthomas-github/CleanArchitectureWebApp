namespace ThirdPartyFreight.Domain.Audits;

public record AuditInfo(DateOnly AuditDateUtc, bool IsAuditActive, DateOnly? AuditCompleteDateUtc);
