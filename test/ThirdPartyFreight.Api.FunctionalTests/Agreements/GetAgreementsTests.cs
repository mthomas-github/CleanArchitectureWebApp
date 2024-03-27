using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Agreements;

public class GetAgreementsTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/agreements";

    [Fact]
    public async Task GetAgreements_ShouldReturnAgreements_WhenAgreementIsFound()
    {

        // Act
        var response = await HttpClient.GetAsync(BaseUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var agreement = await response.Content.ReadFromJsonAsync<List<AgreementResponse>>();
        agreement.Should().NotBeNull();
        agreement!.Count.Should().BeGreaterThan(0);
    }

}