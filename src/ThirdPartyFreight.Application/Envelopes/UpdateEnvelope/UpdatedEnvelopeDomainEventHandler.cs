using DocuSign.eSign.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.DocuSign;
using ThirdPartyFreight.Application.Abstractions.Elsa;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Envelopes.Events;
using Envelope = ThirdPartyFreight.Domain.Envelopes.Envelope;

namespace ThirdPartyFreight.Application.Envelopes.UpdateEnvelope;

internal sealed class UpdatedEnvelopeDomainEventHandler(
    IEnvelopeRepository envelopeRepository,
    IDateTimeProvider dateTimeProvider,
    IApprovalRepository approvalRepository,
    ICarrierRepository carrierRepository,
    IAgreementRepository agreementRepository,
    IUnitOfWork unitOfWork,
    IDocuSignService docuSignService,
    IElsaService elsaService,
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
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Creating Carrier Record");
            throw;
        }
        // Update Agreement
        logger.LogInformation("Updating Agreement Record");
        Agreement result = await agreementRepository.GetByIdAsync(envelope.AgreementId, cancellationToken);
        if (result is not null)
        {
            Agreement.Update(
                result,
                Status.PendingReviewTpf,
                null,
                new ModifiedBy("System"),
                dateTimeProvider.UtcNow
            );
        }

        // Then Save Approval
        logger.LogInformation("Creating Approval Record");
        var approval = Approval.Create(envelope.AgreementId, dateTimeProvider.UtcNow);
        approvalRepository.Add(approval);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
}
