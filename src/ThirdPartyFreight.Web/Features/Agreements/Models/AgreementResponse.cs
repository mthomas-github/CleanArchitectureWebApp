using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class AgreementResponse
{
    public Guid Id { get; set; }
    public int CustomerNumber { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public Status Status { get; set; }
    public AgreementType AgreementType { get; set; }
    public SiteType SiteType { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
    public string? TicketNumber { get; set; }
    public List<SiteResponse> Sites { get; set; } = [];
    public List<DocumentResponse>? Documents { get; set; } = [];
    public List<CarrierResponse>? Carriers { get; set; } = [];
    public List<EnvelopeResponse>? Envelopes { get; set; } = [];
    public List<NoteResponse>? Notes { get; set; } = [];
}
