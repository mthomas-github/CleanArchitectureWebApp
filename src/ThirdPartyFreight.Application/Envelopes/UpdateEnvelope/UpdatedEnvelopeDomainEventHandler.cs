using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Envelopes.Events;

namespace ThirdPartyFreight.Application.Envelopes.UpdateEnvelope;

internal sealed class UpdatedEnvelopeDomainEventHandler(
    IEnvelopeRepository envelopeRepository, 
    IApprovalRepository approvalRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork,
    ILogger<UpdatedEnvelopeDomainEventHandler> logger) 
    : INotificationHandler<EnvelopeUpdatedDomainEvent>
{
    public async Task Handle(EnvelopeUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Envelope Domain Event Handler");
        Envelope? envelope = await envelopeRepository.GetByIdAsync(notification.EnvelopeId, cancellationToken);

        if (envelope is null)
        {
            logger.LogWarning("Envelope with Id {EnvelopeId} was not found", notification.EnvelopeId);
            return;
        }
        
        if (envelope is not { EnvelopeStatus: EnvelopeStatus.Completed })
        {
            
            logger.LogInformation("Envelope with Id {EnvelopeID} was not completed", envelope.Id);
            return;
        }
        logger.LogInformation("Creating Approval Record");
        var approval = Approval.Create(envelope.AgreementId, dateTimeProvider.UtcNow);
        approvalRepository.Add(approval);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Finished Handling Envelope Updated Domain Event");
    }
}
