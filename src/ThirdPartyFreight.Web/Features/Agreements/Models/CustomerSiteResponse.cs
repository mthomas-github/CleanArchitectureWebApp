using Newtonsoft.Json;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class CustomerSiteResponse
{
    public string Customer { get; set; }
    public List<CustomerSite> Sites { get; set; }
    
}
public sealed class CustomerSite
{
    public string Site { get; set; }
    public string SiteAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string FullAddress { get; set; }
}
