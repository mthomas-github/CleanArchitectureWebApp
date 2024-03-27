namespace ThirdPartyFreight.Application.Shared;

public sealed class SiteResponse
{
    public Guid SiteId { get; init; }
    public int SiteNumber { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }
}
