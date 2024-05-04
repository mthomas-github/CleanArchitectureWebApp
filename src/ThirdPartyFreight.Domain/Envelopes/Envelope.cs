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

    public static Envelope Update(
        Guid id,
        EnvelopeStatus envelopeStatus,
        Guid agreementId,
        DateTime createdOnUtc,
        Guid? envelopeId,
        DateTime? LastModifiedDateTime,
        DateTime? InitialSentDateTime,
        DateTime? SentDateTime,
        DateTime? LastStatusChangedDateTime,
        DateTime? CompletedDateTime,
        DateTime? DeliveredDateTime,
        DateTime? ExpiringDateTime,
        DateTime? VoidedDateTime,
        VoidReason? voidReason,
        AutoRespondReason? autoRespondReason
        )
    {
        var envelope = new Envelope
        {
            Id = id,
            EnvelopeStatus = envelopeStatus,
            AgreementId = agreementId,
            CreatedOnUtc = createdOnUtc,
            EnvelopeId = envelopeId,
            LastModifiedOnUtc = LastModifiedDateTime,
            InitialSentOnUtc = InitialSentDateTime,
            SentOnUtc = SentDateTime,
            LastStatusChangedOnUtc = LastStatusChangedDateTime,
            CompletedOnUtc = CompletedDateTime,
            DeliveredOnUtc = DeliveredDateTime,
            ExpiringOnUtc = ExpiringDateTime,
            VoidedOnUtc = VoidedDateTime,
            VoidReason = voidReason,
            AutoRespondReason = autoRespondReason
        };

        return envelope;
    }
}
