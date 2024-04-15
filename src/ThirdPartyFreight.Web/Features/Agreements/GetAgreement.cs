using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Web.Features.Agreements;

public class GetAgreement
{
    public Guid AgreementId { get; set; }
    public string CustomerNumber { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public Status Status { get; set; }
    public AgreementType AgreementType { get; set; }
    public SiteType SiteType { get; set; }
    public EnvelopeStatus EnvelopeStatus { get; set; }
    public string CreatedBy { get; set; }
    public string CsrName { get; set; }
    public string MdmTicket { get; set; }
    public List<Site> Sites { get; set; }
    public List<Document>? Documents { get; set; }
    public List<Carrier>? Carriers { get; set; }
}



