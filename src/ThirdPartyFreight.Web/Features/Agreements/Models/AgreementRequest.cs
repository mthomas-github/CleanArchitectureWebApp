using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class AgreementRequest
{
    public int CustomerNumber { get; set; }
    public string CustomerName { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public Status Status { get; set; }
    public AgreementType AgreementType { get; set; }
    public SiteType SiteType { get; set; }
    public string CreatedBy { get; set; }

}
