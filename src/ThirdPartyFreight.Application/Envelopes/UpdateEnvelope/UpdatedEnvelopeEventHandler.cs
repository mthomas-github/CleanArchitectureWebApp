using MediatR;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Envelopes.Events;

namespace ThirdPartyFreight.Application.Envelopes.UpdateEnvelope;

internal sealed class UpdatedEnvelopeEventHandler(
    IEnvelopeRepository envelopeRepository, 
    IApprovalRepository approvalRepository,
    IDateTimeProvider dateTimeProvider) 
    : INotificationHandler<EnvelopeUpdatedDomainEvent>
{
    public async Task Handle(EnvelopeUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Envelope? envelope = await envelopeRepository.GetByIdAsync(notification.EnvelopeId, cancellationToken);

        if (envelope is not { EnvelopeStatus: EnvelopeStatus.Completed })
        {
            return;
        }

        var approval = Approval.Create(envelope.AgreementId, dateTimeProvider.UtcNow);
        approvalRepository.Add(approval);
    }
}
