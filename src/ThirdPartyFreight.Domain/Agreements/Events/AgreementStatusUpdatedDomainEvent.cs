using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Agreements.Events;

public record AgreementStatusUpdatedDomainEvent(Guid AgreementId, Status NewStatus) : IDomainEvent;
