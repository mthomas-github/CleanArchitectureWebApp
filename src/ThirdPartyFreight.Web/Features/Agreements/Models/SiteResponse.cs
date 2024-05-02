namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class SiteResponse
{
    public Guid SiteId { get; set; }
    public int SiteNumber { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
