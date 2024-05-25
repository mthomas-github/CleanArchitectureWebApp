using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Application.Agreements.GetAgreement;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements.Events;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

public class AgreementUpdatedDomainEventHandler(
    ISender sender, 
    INotificationClient notificationClient, 
    ILogger<AgreementUpdatedDomainEventHandler> logger) 
    : INotificationHandler<AgreementUpdatedDomainEvent>
{
    public async Task Handle(AgreementUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {DomainEvent}", nameof(AgreementUpdatedDomainEventHandler));
        var agreement = new GetAgreementQuery(notification.AgreementId);
        Result<AgreementResponse> response = await sender.Send(agreement, cancellationToken);
        
        if (response.IsSuccess)
        {
            logger.LogInformation("Sending SendAgreementPayload to Hub");
            await notificationClient.SendAgreementPayload(response.Value, cancellationToken);
        }
        logger.LogInformation("Finished Handling {DomainEvent}", nameof(AgreementUpdatedDomainEventHandler));
    }
}
