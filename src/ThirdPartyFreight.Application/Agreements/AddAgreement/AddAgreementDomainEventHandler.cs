using System.Globalization;
using DocuSign.eSign.Model;
using ThirdPartyFreight.Domain.Agreements.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.DocuSign;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Sites;
using Envelope = ThirdPartyFreight.Domain.Envelopes.Envelope;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

internal sealed class AddAgreementDomainEventHandler(
    IAgreementRepository agreementRepository,
    IDocuSignService docuSignService,
    ISiteRepository siteRepository,
    ILogger<AddAgreementDomainEventHandler> logger,
    IUnitOfWork unitOfWork,
    IEnvelopeRepository envelopeRepository,
    IDateTimeProvider dateTimeProvider)
    : INotificationHandler<AgreementCreatedDomainEvent>
{
    public async Task Handle(AgreementCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Step 1 Pull the agreement from the database
        logger.LogInformation("Handling AgreementCreatedDomainEvent for AgreementId: {AgreementId}", notification.AgreementId);
        Agreement? result = await agreementRepository.GetByIdAsync(notification.AgreementId, cancellationToken);
        if (result is not null)
        {
            // Step 2 Get Sites
            logger.LogInformation("Getting Sites for AgreementId: {AgreementId}", result.Id);
            IEnumerable<Site> sites =
                await siteRepository.GetSitesAsyncByAgreementId(notification.AgreementId, cancellationToken);
            string siteString = string.Join(", ", sites.Select(s => s.SiteNumber.Value));
            string customerNum = result.ContactInfo.CustomerNumber.ToString(CultureInfo.CurrentCulture);
            // Step 3 Call DocuSign
            logger.LogInformation("Calling DocuSign for AgreementId: {AgreementId}", result.Id);
            EnvelopeSummary response = await docuSignService.SendEnvelopeFromTemplate(result.ContactInfo.CustomerEmail,
                result.ContactInfo.CustomerName, "72ec3391-33b4-4cb3-a131-210a0f8d262a",
                customerNumber: customerNum, result.ContactInfo.CompanyName, siteString);

            if (response.Status == "sent")
            {
                logger.LogInformation("Calling DocuSign for AgreementId: {AgreementId}", result.Id);
                string? envelopeId = response.EnvelopeId;
                string? sentDate = response.StatusDateTime;
                
                Envelope envelope = await envelopeRepository.GetEnvelopeAsyncByAgreementId(result.Id, cancellationToken);

                Envelope.Update(
                    envelope,
                    EnvelopeStatus.Sent,
                    Guid.Parse(response.EnvelopeId),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.CurrentCulture),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.CurrentCulture),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.CurrentCulture),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.CurrentCulture),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

                Agreement.Update(
                    result,
                    Status.CustomerSignature,
                    null,
                    new ModifiedBy("System"),
                    dateTimeProvider.UtcNow
                );

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                logger.LogError("Error sending envelope for AgreementId: {AgreementId}", result.Id);
            }
            logger.LogInformation("Finished handling AgreementCreatedDomainEvent for AgreementId: {AgreementId}", notification.AgreementId);
        }

        // Step 2 Get Sites.
        logger.LogError("Agreement with id: {AgreementId} not found", notification.AgreementId);
    }
}
