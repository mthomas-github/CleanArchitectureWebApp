using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Shared;
using ThirdPartyFreight.Domain.Sites.Events;

namespace ThirdPartyFreight.Domain.Sites;

public sealed class Site : Entity
{
    private Site(
        Guid id,
        Guid agreementId,
        SiteNumber siteNumber,
        Address siteAddress)
        : base(id)
    {
        AgreementId = agreementId;
        SiteNumber = siteNumber;
        SiteAddress = siteAddress;
    }

    private Site() { }

    public Guid AgreementId { get; private set; }
    public SiteNumber SiteNumber { get; private set; }
    public Address SiteAddress { get; private set; }

    public static Site Create(
        Guid agreementId,
        SiteNumber siteNumber,
        Address siteAddress)
    {
        var site = new Site(
                       Guid.NewGuid(),
                                  agreementId,
                                  siteNumber,
                                  siteAddress);

        site.RaiseDomainEvent(new SiteCreatedDomainEvent(site.Id));

        return site;
    }

}