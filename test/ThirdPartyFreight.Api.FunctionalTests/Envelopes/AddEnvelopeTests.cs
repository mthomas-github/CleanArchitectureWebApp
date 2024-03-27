using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Envelopes;
using ThirdPartyFreight.Domain.Envelopes;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Envelopes;

public class AddEnvelopeTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/envelopes";
    private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";

    [Theory]
    [InlineData(EmptyGuid, EnvelopeStatus.Draft)]
    [InlineData("d78c0624-4f9e-4f3e-aa4f-7e49f93bbbd7", (EnvelopeStatus)558)]
    [InlineData(EmptyGuid, (EnvelopeStatus)100)]
    public async Task AddEnvelope_ShouldReturnBadRequest_WhenEnvelopeRequestIsInvalid(
        string agreementId,
        EnvelopeStatus envelopeStatus)
    {
        // Arrange
        var addEnvelopeRequest = new AddEnvelopeRequest(envelopeStatus, Guid.Parse(agreementId));

        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, addEnvelopeRequest);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddEnvelope_ShouldReturnOK_WhenEnvelopeIsAdded()
    {
        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, EnvelopeData.AddTestEnvelopeRequest);

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
    }
}