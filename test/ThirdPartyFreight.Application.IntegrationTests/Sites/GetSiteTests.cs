using ThirdPartyFreight.Application.Sites.GetSite;
using ThirdPartyFreight.Domain.Sites;
using FluentAssertions;
using ThirdPartyFreight.Application.IntegrationTests.Infrastructure;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.IntegrationTests.Sites;

public class GetSiteTests : BaseIntegrationTest
{
    private static readonly Guid SiteId = Guid.NewGuid();
    public GetSiteTests(IntegrationTestWebAppFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task GetSite_ShouldReturnFailure_WhenAgreementIsNotFound()
    {
        // Arrange
        var query = new GetSiteQuery(SiteId);

        // Act
        Result<SiteResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(SiteErrors.NotFound);
        ;        }
}
