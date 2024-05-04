using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class EnvelopeResponse
{
    public Guid? EnvelopeId { get; set; }
    public Guid? AgreementId { get; set; }
    public EnvelopeStatus EnvelopeStatus { get; set; }
    public Guid? DocuSignId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOnUtc { get; set; }
    public DateTime? InitialSentOnUtc { get; set; }
    public DateTime? SentOnUtc { get; set; }
    public DateTime? LastStatusChangedOnUtc { get; set; }
    public DateTime? CompletedOnUtc { get; set; }
    public DateTime? DeliveredOnUtc { get; set; }
    public DateTime? ExpiringOnUtc { get; set; }
    public DateTime? VoidedOnUtc { get; set; }
    public string? VoidReason { get; set; }
    public string? AutoRespondReason { get; set; }

}
