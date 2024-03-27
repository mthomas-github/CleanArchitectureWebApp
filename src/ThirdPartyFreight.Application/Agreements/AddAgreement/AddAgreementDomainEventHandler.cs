using ThirdPartyFreight.Domain.Agreements.Events;
using MediatR;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

internal sealed class AddAgreementDomainEventHandler : INotificationHandler<AgreementCreatedDomainEvent>
{
    public Task Handle(AgreementCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
