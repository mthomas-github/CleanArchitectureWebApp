
using Newtonsoft.Json;

namespace ThirdPartyFreight.Application.Shared;

public sealed class CustomerSiteResponse
{
    [JsonProperty("Customer #")]
    public string Customer { get; init; }
    public List<CustomerSite> Sites { get; init; }

}
public sealed class CustomerSite
{
    [JsonProperty("Site #")]
    public string Site { get; init; }
    public string SiteAddress { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }
    public string FullAddress { get; init; }   
}


