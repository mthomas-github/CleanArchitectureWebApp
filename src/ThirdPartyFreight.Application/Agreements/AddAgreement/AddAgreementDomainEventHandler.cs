using System.Globalization;
using DocuSign.eSign.Model;
using ThirdPartyFreight.Domain.Agreements.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.DocuSign;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Application.Agreements.GetAgreement;
using ThirdPartyFreight.Application.Shared;
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
        Agreement? agreement = await agreementRepository.GetByIdAsync(notification.AgreementId, cancellationToken);
        if (agreement is not null)
        {
            // Step 2 Get Sites
            logger.LogInformation("Getting Sites for AgreementId: {AgreementId}", agreement.Id);
            IEnumerable<Site> sites =
                await siteRepository.GetSitesAsyncByAgreementId(notification.AgreementId, cancellationToken);
            string siteString = string.Join(", ", sites.Select(s => s.SiteNumber.Value));
            string customerNum = agreement.ContactInfo.CustomerNumber.ToString(CultureInfo.CurrentCulture);
            // Step 3 Call DocuSign
            logger.LogInformation("Calling DocuSign for AgreementId: {AgreementId}", agreement.Id);
            EnvelopeSummary response = await docuSignService.SendEnvelopeFromTemplate(agreement.ContactInfo.CustomerEmail,
                agreement.ContactInfo.CustomerName, "72ec3391-33b4-4cb3-a131-210a0f8d262a",
                customerNumber: customerNum, agreement.ContactInfo.CompanyName, siteString);

            if (response.Status == "sent")
            {
                string? envelopeId = response.EnvelopeId;
                string? sentDate = response.StatusDateTime;
                
                Envelope envelope = await envelopeRepository.GetEnvelopeAsyncByAgreementId(agreement.Id, cancellationToken);
                
                try
                {
                    logger.LogInformation("Updating Status for AgreementId: {AgreementId}", agreement.Id);
                    agreement.SetStatus(Status.CustomerSignature, dateTimeProvider.UtcNow);
                }
                catch (Exception ex)
                {
                    logger.LogError("Issue Updating Agreement For {AgreementId} With {Error}", agreement.Id, ex.Message);
                }

                envelope.SetUpdatedValues(
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
                
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                logger.LogError("Error sending envelope for AgreementId: {AgreementId}", agreement.Id);
            }
            logger.LogInformation("Finished handling AgreementCreatedDomainEvent for AgreementId: {AgreementId}", notification.AgreementId);
        }
        else
        {
            // Step 2 Get Sites.
            logger.LogError("Agreement with id: {AgreementId} not found", notification.AgreementId);   
        }
    }
}
