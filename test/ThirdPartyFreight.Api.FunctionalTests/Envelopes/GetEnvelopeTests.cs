using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Envelopes;

public class GetEnvelopeTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/envelopes";

    [Fact]
    public async Task GetEnvelope_ShouldReturnOK_WhenEnvelopeIsRetrieved()
    {
        // Arrange
        var envelopeId = new Guid("F43FFD1B-DC67-46CC-BE02-D033C65A5395");
        
        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}/{envelopeId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var envelope = await response.Content.ReadFromJsonAsync<EnvelopeResponse>();
        envelope.Should().NotBeNull();
        envelope!.EnvelopeId.Should().Be(envelopeId);
    }

    [Fact]
    public async Task GetEnvelope_ShouldReturnNotFound_WhenEnvelopeIsNotFound()
    {
        // Arrange
        var envelopeId = Guid.NewGuid();
        
        // Act
        var response = await HttpClient.GetAsync($"{BaseUrl}/{envelopeId}");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}