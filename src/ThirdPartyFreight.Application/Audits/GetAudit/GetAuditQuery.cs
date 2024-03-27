using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Audits.GetAudit;

public sealed record GetAuditQuery(Guid AuditId) : IQuery<AuditResponse>;

