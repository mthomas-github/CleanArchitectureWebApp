using System.ComponentModel;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Web.Extensions;

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
    public EnvelopeResponse? Envelope { get; set; }
    public List<NoteResponse>? Notes { get; set; } = [];
    
    public string StatusDisplayName => GetEnumDisplayName.GetDisplayName(Status);
}

public enum Status
{
    [Description("Agreement Being Created")]
    Creating = 1,
    [Description("Waiting On Customers Email")]
    CustomerResponse = 2,
    [Description("Waiting On Customers Signature")]
    CustomerSignature = 3,
    [Description("With TPF Team")]
    PendingReviewTpf = 4,
    [Description("With TMS Team")]
    PendingReviewTms = 5,
    [Description("With MDM Team")]
    PendingReviewMdm = 6,
    [Description("All Teams Approved")]
    Completed = 7,
    Closed = 8,
    [Description("Issue!")]
    Failed = 9,
    Cancelled = 10,
    [Description("Customer Refused To Sign")]
    CustomerRejected = 11,
    [Description("Failed Reviw Process")]
    ApprovalRejected = 12
}
