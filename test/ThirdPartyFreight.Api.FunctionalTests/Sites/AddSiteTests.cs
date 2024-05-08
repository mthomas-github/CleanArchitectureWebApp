using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Sites;
using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Sites;

public class AddSiteTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/sites";
    private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";

    [Fact]
    public async Task AddSite_ShouldReturnOK_WhenSiteIsAdded()
    {
        // Act
        HttpResponseMessage? response = await HttpClient.PostAsJsonAsync(BaseUrl, SiteData.AddSiteRequest);

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData(EmptyGuid, "123", "", "Any town", "NY", "12345")]
    [InlineData("d78c0624-4f9e-4f3e-aa4f-7e49f93bbbd7", "123", "123 Main St", "", "NY", "12345")]
    [InlineData("d78c0624-4f9e-4f3e-aa4f-7e49f93bbbd7", "", "", "", "", "")]

    public async Task AddSite_ShouldReturnBadRequest_WhenSiteRequestIsInvalid(
        string agreementId,
        string siteNumber,
        string siteAddress,
        string siteCity,
        string siteState,
        string siteZipCode)
    {
        // Arrange
        var addSiteRequest = new AddSiteRequest(new Guid(agreementId), siteNumber, siteAddress, siteCity, siteState, siteZipCode);

        // Act
        HttpResponseMessage? response = await HttpClient.PostAsJsonAsync(BaseUrl, addSiteRequest);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}