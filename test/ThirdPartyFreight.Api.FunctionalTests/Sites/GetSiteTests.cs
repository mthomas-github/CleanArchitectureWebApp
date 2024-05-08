using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Sites;

public class GetSiteTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/sites";

    [Fact]
    public async Task GetSite_ShouldReturnOK_WhenSiteIsRetrieved()
    {
        // Arrange
        var siteId = new Guid("EF765A0C-20B1-4D67-A718-1DBBC641047C");
        
        // Act
        HttpResponseMessage? response = await HttpClient.GetAsync($"{BaseUrl}/{siteId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        SiteResponse? site = await response.Content.ReadFromJsonAsync<SiteResponse>();
        site.Should().NotBeNull();
        site!.SiteId.Should().Be(siteId);
    }

    [Fact]
    public async Task GetSite_ShouldReturnNotFound_WhenSiteIsNotFound()
    {
        // Arrange
        var siteId = Guid.NewGuid();
        
        // Act
        HttpResponseMessage? response = await HttpClient.GetAsync($"{BaseUrl}/{siteId}");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}