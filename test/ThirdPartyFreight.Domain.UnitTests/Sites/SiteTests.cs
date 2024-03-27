using ThirdPartyFreight.Domain.Sites;
using ThirdPartyFreight.Domain.Sites.Events;
using FluentAssertions;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Sites;

public class SiteTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {

        // Act
        var site = Site.Create(SiteData.AgreementId, SiteData.SiteNumber, SiteData.SiteAddress);

        // Assert
        site.Id.Should().NotBeEmpty();
        site.AgreementId.Should().Be(SiteData.AgreementId);
        site.SiteNumber.Should().Be(SiteData.SiteNumber);
        site.SiteAddress.Should().Be(SiteData.SiteAddress);
    }

    [Fact]
    public void Create_Should_RaiseAgreementCreatedDomainEvent()
    {
        // Act
        var site = Site.Create(SiteData.AgreementId, SiteData.SiteNumber, SiteData.SiteAddress);

        // Assert
        var domainEvents = AssertDomainEventWasPublished<SiteCreatedDomainEvent>(site);

        domainEvents.SiteId.Should().Be(site.Id);

    }
}