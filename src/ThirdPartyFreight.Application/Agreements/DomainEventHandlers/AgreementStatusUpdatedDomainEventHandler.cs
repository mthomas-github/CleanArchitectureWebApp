using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Domain.Agreements.Events;

namespace ThirdPartyFreight.Application.Agreements.DomainEventHandlers;

public class AgreementStatusUpdatedDomainEventHandler(
    INotificationClient notificationClient, 
    ILogger<AgreementStatusUpdatedDomainEventHandler> logger) 
    : INotificationHandler<AgreementStatusUpdatedDomainEvent>
{
    public async Task Handle(AgreementStatusUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {DomainEvent}", nameof(AgreementStatusUpdatedDomainEventHandler));
        logger.LogInformation("Sending SendAgreementPayload to Hub");
        await notificationClient.SendAgreementPayload(notification.AgreementId, notification.NewStatus, cancellationToken);
        logger.LogInformation("Finished Handling {DomainEvent}", nameof(AgreementStatusUpdatedDomainEventHandler));
    }
}
