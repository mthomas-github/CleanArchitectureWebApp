using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Audits.AddAudit;

public sealed record AddAuditCommand(Guid AgreementId, DateOnly AuditDateDue, bool IsAuditActive) : ICommand;
