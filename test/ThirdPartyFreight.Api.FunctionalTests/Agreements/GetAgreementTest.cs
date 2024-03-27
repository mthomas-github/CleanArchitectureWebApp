using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Agreements;

public class GetAgreementTest(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/agreements";

    [Fact]
    public async Task GetAgreement_ShouldReturnNotFound_WhenAgreementIsNotFound()
    {
        var agreementId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}/{agreementId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAgreement_ShouldReturnAgreement_WhenAgreementIsFound()
    {
        // Arrange
        var agreementId = Guid.Parse("BBEA553F-7C2F-4818-9669-650DB74DF39F");

        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}/{agreementId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var agreement = await response.Content.ReadFromJsonAsync<AgreementResponse>();
        agreement.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAgreement_ShouldReturnAgreement_WithArrayOfSites()
    {
        // Arrange
        var agreementId = Guid.Parse("BBEA553F-7C2F-4818-9669-650DB74DF39F");
        const int expectedSiteCount = 2;

        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}/{agreementId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var agreement = await response.Content.ReadFromJsonAsync<AgreementResponse>();
        agreement.Should().NotBeNull();
        agreement!.Sites.Should().NotBeNullOrEmpty();
        agreement.Sites.Count.Should().Be(expectedSiteCount);
    }
}