using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Envelopes.Events;

namespace ThirdPartyFreight.Domain.Envelopes;

public sealed class Envelope : Entity
{
    private Envelope(
        Guid id,
        EnvelopeStatus envelopeStatus,
        Guid agreementId,
        DateTime createdOnUtc
        )
        : base(id)
    {
        EnvelopeStatus = envelopeStatus;
        AgreementId = agreementId;
        CreatedOnUtc = createdOnUtc;
    }

    private Envelope()
    {
    }

    public EnvelopeStatus EnvelopeStatus { get; private set; }
    public Guid AgreementId { get; private set; }
    public Guid? EnvelopeId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? LastModifiedOnUtc { get; private set; }
    public DateTime? InitialSentOnUtc { get; private set; }
    public DateTime? SentOnUtc { get; private set; }
    public DateTime? LastStatusChangedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? DeliveredOnUtc { get; private set; }
    public DateTime? ExpiringOnUtc { get; private set; }
    public DateTime? VoidedOnUtc { get; private set; }
    public VoidReason? VoidReason { get; private set; }
    public AutoRespondReason? AutoRespondReason { get; private set; }


    public static Envelope Create(
        EnvelopeStatus envelopeStatus,
        Guid agreementId,
        DateTime createdOnUtc
    )
    {
        var envelope = new Envelope(
                       Guid.NewGuid(),
                                  envelopeStatus,
                                  agreementId,
                                  createdOnUtc
                              );

        envelope.RaiseDomainEvent(new EnvelopeCreatedDomainEvent(envelope.Id));

        return envelope;
    }

    public Result SetUpdatedValues(
        EnvelopeStatus envelopeStatus,
        Guid? envelopeId,
        DateTime? lastModifiedDateTime,
        DateTime? initialSentDateTime,
        DateTime? sentDateTime,
        DateTime? lastStatusChangedDateTime,
        DateTime? completedDateTime,
        DateTime? deliveredDateTime,
        DateTime? expiringDateTime,
        DateTime? voidedDateTime,
        VoidReason? voidReason,
        AutoRespondReason? autoRespondReason)
    {
        EnvelopeStatus = envelopeStatus;
        EnvelopeId = envelopeId;
        LastModifiedOnUtc = lastModifiedDateTime;
        InitialSentOnUtc = initialSentDateTime;
        SentOnUtc = sentDateTime;
        LastStatusChangedOnUtc = lastStatusChangedDateTime;
        CompletedOnUtc = completedDateTime;
        ExpiringOnUtc = expiringDateTime;
        VoidedOnUtc = voidedDateTime;
        VoidReason = voidReason;
        DeliveredOnUtc = deliveredDateTime;
        AutoRespondReason = autoRespondReason;
        
        RaiseDomainEvent(new EnvelopeUpdatedDomainEvent(Id));

        return Result.Success();
    }
   
}
