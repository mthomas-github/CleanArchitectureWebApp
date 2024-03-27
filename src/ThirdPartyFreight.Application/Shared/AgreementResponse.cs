using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Application.Shared;

public sealed class AgreementResponse
{
    public Guid Id { get; init; }
    public int CustomerNumber { get; init; }
    public string BusinessName { get; init; } = string.Empty;
    public string ContactName { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public Status Status { get; init; }
    public AgreementType AgreementType { get; init; }
    public SiteType SiteType { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? ModifiedOnUtc { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? ModifiedBy { get; init; }
    public string? TicketNumber { get; init; }
    public List<SiteResponse> Sites { get; init; } = [];
    public List<DocumentResponse>? Documents { get; init; } = [];
    public List<CarrierResponse>? Carriers { get; init; } = [];
    public List<EnvelopeResponse>? Envelopes { get; init; } = [];
    public List<NoteResponse>? Notes { get; init; } = [];
}
