using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Application.Shared;

public sealed class EnvelopeResponse
{
    public Guid Id { get; init; }
    public EnvelopeStatus? EnvelopeStatus { get; init; }
    public Guid? EnvelopeId { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? LastModifiedOnUtc { get; init; }
    public DateTime? InitialSentOnUtc { get; init; }
    public DateTime? SentOnUtc { get; init; }
    public DateTime? LastStatusChangedOnUtc { get; init; }
    public DateTime? CompletedOnUtc { get; init; }
    public DateTime? DeliveredOnUtc { get; init; }
    public DateTime? ExpiringOnUtc { get; init; }
    public DateTime? VoidedOnUtc { get; init; }
    public string? VoidReason { get; init; }
    public string? AutoRespondReason { get; init; }

}
