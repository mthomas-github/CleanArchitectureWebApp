namespace ThirdPartyFreight.Api.Controllers.Audits;

public sealed record AddAuditRequest(Guid AgreementId, DateOnly AuditDateUtc, bool IsAuditActive);
