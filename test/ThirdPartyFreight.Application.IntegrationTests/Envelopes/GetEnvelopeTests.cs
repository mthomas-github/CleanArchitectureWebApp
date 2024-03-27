using ThirdPartyFreight.Application.Envelopes.GetEnvelope;
using ThirdPartyFreight.Domain.Envelopes;
using FluentAssertions;
using ThirdPartyFreight.Application.IntegrationTests.Infrastructure;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.IntegrationTests.Envelopes;

public class GetEnvelopeTests : BaseIntegrationTest
{
    private static readonly Guid EnvelopeId = Guid.NewGuid();
    public GetEnvelopeTests(IntegrationTestWebAppFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task GetEnvelope_ShouldReturnFailure_WhenAgreementIsNotFound()
    {
        // Arrange
        var query = new GetEnvelopeQuery(EnvelopeId);

        // Act
        Result<EnvelopeResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(EnvelopeErrors.NotFound);
        ;        }
}
