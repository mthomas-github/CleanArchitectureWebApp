using System.Globalization;
using DocuSign.eSign.Model;
using ThirdPartyFreight.Domain.Agreements.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Sites;
using Envelope = ThirdPartyFreight.Domain.Envelopes.Envelope;
using System;

namespace ThirdPartyFreight.Infrastructure.DocuSign;

internal sealed class AddAgreementDomainEventHandler : INotificationHandler<AgreementCreatedDomainEvent>
{
    private readonly IAgreementRepository _agreementRepository;
    private readonly ISiteRepository _siteRepository;
    private readonly IDocuSignService _docuSignService;
    private readonly IEnvelopeRepository _envelopeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddAgreementDomainEventHandler> _logger;

    public AddAgreementDomainEventHandler(
        IAgreementRepository agreementRepository, 
        IDocuSignService docuSignService, 
        ISiteRepository siteRepository, 
        ILogger<AddAgreementDomainEventHandler> logger, 
        IUnitOfWork unitOfWork, 
        IEnvelopeRepository envelopeRepository)
    {
        _agreementRepository = agreementRepository;
        _docuSignService = docuSignService;
        _siteRepository = siteRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _envelopeRepository = envelopeRepository;
    }

    public async Task Handle(AgreementCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Step 1 Pull the agreement from the database
        _logger.LogInformation("Handling AgreementCreatedDomainEvent for AgreementId: {AgreementId}", notification.AgreementId);
        Agreement? result = await _agreementRepository.GetByIdAsync(notification.AgreementId, cancellationToken);
        if (result is not null)
        {
            // Step 2 Get Sites
            _logger.LogInformation("Getting Sites for AgreementId: {AgreementId}", result.Id);
            IEnumerable<Site> sites =
                await _siteRepository.GetSitesAsyncByAgreementId(notification.AgreementId, cancellationToken);
            string siteString = string.Join(", ", sites.Select(s => s.SiteNumber));
            string customerNum = result.ContactInfo.CustomerNumber.ToString(CultureInfo.CurrentCulture);
            // Step 3 Call DocuSign
            _logger.LogInformation("Calling DocuSign for AgreementId: {AgreementId}", result.Id);
            EnvelopeSummary response = await _docuSignService.SendEnvelopeFromTemplate(result.ContactInfo.CustomerEmail,
                result.ContactInfo.CustomerName, "72ec3391-33b4-4cb3-a131-210a0f8d262a",
                customerNumber: customerNum, result.ContactInfo.CompanyName, siteString);

            if (response.Status == "sent")
            {
                _logger.LogInformation("Calling DocuSign for AgreementId: {AgreementId}", result.Id);
                string? envelopeId = response.EnvelopeId;
                string? sentDate = response.StatusDateTime;
                Envelope envelope = await _envelopeRepository.GetEnvelopeAsyncByAgreementId(result.Id, cancellationToken);
                
                var newEnvelope = Envelope.Update(
                    envelope.Id, 
                    EnvelopeStatus.Sent, 
                    result.Id, 
                    envelope.CreatedOnUtc, 
                    Guid.Parse(response.EnvelopeId), 
                    DateTime.Parse(response.StatusDateTime, CultureInfo.InvariantCulture),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.InvariantCulture),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.InvariantCulture),
                    DateTime.Parse(response.StatusDateTime, CultureInfo.InvariantCulture), 
                    null, null, null, null, null, null);

                _envelopeRepository.Update(newEnvelope);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                _logger.LogError("Error sending envelope for AgreementId: {AgreementId}", result.Id);
            }
            _logger.LogInformation("Finished handling AgreementCreatedDomainEvent for AgreementId: {AgreementId}", notification.AgreementId);
        }

        // Step 2 Get Sites.
        _logger.LogError("Agreement with id: {AgreementId} not found", notification.AgreementId);
    }
}
