using DocuSign.eSign.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.DocuSign;
using ThirdPartyFreight.Application.Abstractions.Elsa;
using ThirdPartyFreight.Application.Abstractions.Excel;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Domain.Documents;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Envelopes.Events;
using Document = ThirdPartyFreight.Domain.Documents.Document;
using Envelope = ThirdPartyFreight.Domain.Envelopes.Envelope;

namespace ThirdPartyFreight.Application.Envelopes.DomainEventHandlers;

internal sealed class UpdatedEnvelopeDomainEventHandler(
    IEnvelopeRepository envelopeRepository,
    IDateTimeProvider dateTimeProvider,
    IApprovalRepository approvalRepository,
    ICarrierRepository carrierRepository,
    IAgreementRepository agreementRepository,
    IDocumentRepository documentRepository,
    IUnitOfWork unitOfWork,
    IDocuSignService docuSignService,
    IElsaService elsaService,
    IExcelService excelService,
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

        // Need To Call DocuSign To Get Data From Envelope
        EnvelopeFormData completedEnv = await docuSignService.GetEnvelopeFormData(envelope.EnvelopeId.ToString()!);
        
        if (completedEnv is null)
        {
            logger.LogWarning("Envelope with Id {EnvelopeId} did return any data from DocuSing", envelope.Id);
            throw new NullReferenceException("Envelope did not return any data from DocuSing");
        }

        // Create Carrier
        logger.LogInformation("Creating Carrier Record");
        try
        {
            string primaryCarrierName = GetValue(completedEnv, CarrierFormFieldNames.PrimaryCarrierName);
            string primaryCarrierAcct = GetValue(completedEnv, CarrierFormFieldNames.PrimaryCarrierNumber);
            string ltlCarrierName = GetValue(completedEnv, CarrierFormFieldNames.PrimaryLtlName);
            string ltlCarrierAcct = GetValue(completedEnv, CarrierFormFieldNames.PrimaryLtlBillTo);
            string ltlAddress = GetValue(completedEnv, CarrierFormFieldNames.PrimaryLtlAddress);
            string ltlCity = GetValue(completedEnv, CarrierFormFieldNames.PrimaryLtlCity);
            string ltlState = GetValue(completedEnv, CarrierFormFieldNames.PrimaryLtlState);
            string ltlZip = GetValue(completedEnv, CarrierFormFieldNames.PrimaryLtlZip);
            string address = ltlAddress + ", " + ltlCity + ", " + ltlState + " " + ltlZip;

            // First Primary Carrier
            var primary = Carrier.Create(
                envelope.AgreementId,
                new CarrierInfo(
                    primaryCarrierName,
                    primaryCarrierAcct,
                    "",
                    CarrierType.Parcel));
            carrierRepository.Add(primary);
            //First Ltl Carrier
            var primaryLtl = Carrier.Create(
                envelope.AgreementId,
                new CarrierInfo(
                    ltlCarrierName,
                    ltlCarrierAcct,
                    address,
                    CarrierType.Ltl));
            carrierRepository.Add(primaryLtl);
            
            // Update Agreement
            logger.LogInformation("Updating Agreement Record");
            Agreement result = await agreementRepository.GetByIdAsync(envelope.AgreementId, cancellationToken);
            result?.SetStatus(Status.PendingReviewTpf, dateTimeProvider.UtcNow);

            // Then Save Approval
            logger.LogInformation("Creating Approval Record");
            var approval = Approval.Create(envelope.AgreementId, dateTimeProvider.UtcNow);
            approvalRepository.Add(approval);
        
            // Save Signed Agreement
            logger.LogInformation("Creating Document Record");
            // Make Change To Get DocumentId From AppSettings
            Stream signedDoc = await docuSignService.GetDocumentById(completedEnv.EnvelopeId, "1");
            string singedDocBase64 = await StreamToBase64String(signedDoc);
            var docRec = Document.Create(result!.Id, new Details(
                "Third-Party Freight Agreement",
                singedDocBase64,
                DocumentType.Agreement));
            documentRepository.Add(docRec);
            string customerName = GetValue(completedEnv, CarrierFormFieldNames.CustomerName);
            string customerNumber = GetValue(completedEnv, CarrierFormFieldNames.CustomerNumber);
            string shipToSitesNumbers = GetValue(completedEnv, CarrierFormFieldNames.SiteNumbers);
            
            // Create Routing Guid
            logger.LogInformation("Creating Routing Guide");
            var routingGuideDataList = new List<RoutingGuideData>
            {
                new()
                {
                    CustomerName = customerName,
                    CustomerNumber = customerNumber,
                    ParcelCarrierName = primaryCarrierName,
                    ParcelCarrierAcct = primaryCarrierAcct,
                    SecondaryParcelCarrierName = primaryCarrierName,
                    SecondaryParcelCarrierAcct = primaryCarrierAcct,
                    LtlBillTo = ltlCarrierAcct,
                    LtlCarrierName = ltlCarrierName,
                    LtlAddress = ltlAddress,
                    LtlCity = ltlCity,
                    LtlState = ltlState,
                    LtlZipcode = ltlZip,
                    ShipToSites = shipToSitesNumbers
                }
            };
            
            string routingGuideBase64 = excelService.CreateRoutingGuide(routingGuideDataList);
            
            var routingGuide = Document.Create(result.Id, new Details(
                $"Routing Guide For {customerName}",
                routingGuideBase64,
                DocumentType.RoutingGuide));
            documentRepository.Add(routingGuide);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Creating Carrier Record");
            throw;
        }
        
        //Call Elsa To Fire Off Process
        logger.LogInformation("Calling Elsa To Start WorkFlow Process");
        await elsaService.ExecuteTask(envelope.AgreementId.ToString(), cancellationToken);
        logger.LogInformation("Finished Elsa To Start WorkFlow Process");

        logger.LogInformation("Finished Handling Envelope Updated Domain Event");
    }

    private static string GetValue(EnvelopeFormData envelopeFormData, string fieldName)
    {
        FormDataItem? field = envelopeFormData.FormData?.FirstOrDefault(f => f.Name == fieldName);
        return field?.Value;
    }

    private static async Task<string> StreamToBase64String(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        byte[] byteArray = memoryStream.ToArray();
        return Convert.ToBase64String(byteArray);
    }
}
