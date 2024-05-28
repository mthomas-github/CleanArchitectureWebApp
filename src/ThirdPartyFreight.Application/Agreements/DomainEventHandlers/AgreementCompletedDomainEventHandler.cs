using System.Net.Mail;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Email;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Agreements.Events;
using ThirdPartyFreight.Domain.Audits;
using ThirdPartyFreight.Domain.Documents;
using ThirdPartyFreight.Domain.Users;

namespace ThirdPartyFreight.Application.Agreements.DomainEventHandlers;

public class AgreementCompletedDomainEventHandler(
    IAgreementRepository agreementRepository,
    IDocumentRepository documentRepository,
    IAuditRepository auditRepository,
    INotificationClient notificationClient,
    IEmailService emailService,
    IUnitOfWork unitOfWork, 
    IDateTimeProvider dateTimeProvider, 
    ILogger<AgreementCompletedDomainEventHandler> logger) 
    : INotificationHandler<AgreementCompletedDomainEvent>
{
    public async Task Handle(AgreementCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Step 1 Pull Agreement Record, To Retrieve Customer Email, Name
        Agreement agreement = await agreementRepository.GetByIdAsync(notification.AgreementId, cancellationToken);

        if (agreement is null)
        {
            logger.LogError("Unable To locate AgreementId {AgreementId}", notification.AgreementId);
            throw new NullReferenceException("Unable To Find Agreement in");
        }

        string customerName = agreement.ContactInfo.CustomerName;
        string customerEmail = agreement.ContactInfo.CustomerEmail;
        int customerNumber = agreement.ContactInfo.CustomerNumber;
        string businessName = agreement.ContactInfo.CompanyName;

        string emailBody = $"""
                            Hello {customerName},

                            We have completed your request to modify your third-party freight agreement with Beckman Coulter.

                            Please don't hesitate to contact us if you have any further questions or need clarification on this request.

                            Best regards,
                            Third-Party Freight Administration Team
                            1-800-526-3821, option 2
                            thirdpartybilling@beckman.com
                            """;
        string emailSubject = $"Beckman Third Party Freight Agreement for: {businessName} has been completed";
        // Step 2 Email Client
        await emailService.SendEmailAsync(
            customerEmail,
            emailSubject,
            emailBody
        );
        // Step 3 Email Records Management
        // Step 3.1 Get Records
        Document? documents = await documentRepository.GetByIdAsync(new Guid(), cancellationToken);
        await emailService.SendEmailAsync("", "", "");
        // Step 4 Add Audit Record
        var newAudit = Audit.Create(
            notification.AgreementId, 
            new AuditInfo(DateOnly.FromDateTime(dateTimeProvider.UtcNow.AddYears(1)), false, null));
        auditRepository.Add(newAudit);
        // Step 5 Send Status Update To Client
        await notificationClient.SendAgreementPayload(notification.AgreementId, Status.Closed, cancellationToken);
        // Step 6 Close Out Agreement Request
        agreement.Close(dateTimeProvider.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
