using ThirdPartyFreight.Domain.Agreements.Events;
using MediatR;

namespace ThirdPartyFreight.Infrastructure.DocuSign;

internal sealed class AddAgreementDomainEventHandler : INotificationHandler<AgreementCreatedDomainEvent>
{
    public Task Handle(AgreementCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Send agreement to DocuSign

        return Task.CompletedTask;
    }
}
