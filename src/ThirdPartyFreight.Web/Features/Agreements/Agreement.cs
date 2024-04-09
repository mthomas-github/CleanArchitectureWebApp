namespace ThirdPartyFreight.Web.Features.Agreements;

public class Agreement
{
    public int AgreementId { get; set; }
    public string CustomerNumber { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string Status { get; set; }
    public string AgreementType { get; set; }
    public string SiteType { get; set; }
    public string EnvelopeStatus { get; set; }
    public string CreatedBy { get; set; }
    public string CsrName { get; set; }
    public string MdmTicket { get; set; }
    public List<Site> Sites { get; set; }
    public List<Document> Documents { get; set; }
    public List<Carrier> Carriers { get; set; }
}
