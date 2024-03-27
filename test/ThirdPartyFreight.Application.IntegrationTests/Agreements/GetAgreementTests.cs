using ThirdPartyFreight.Application.Agreements.GetAgreement;
using ThirdPartyFreight.Domain.Agreements;
using FluentAssertions;
using ThirdPartyFreight.Application.IntegrationTests.Infrastructure;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.IntegrationTests.Agreements;

public class GetAgreementTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly Guid AgreementId = Guid.NewGuid();

    [Fact]
    public async Task GetAgreement_ShouldReturnFailure_WhenAgreementIsNotFound()
    {
        // Arrange
        var query = new GetAgreementQuery(AgreementId);

        // Act
        Result<AgreementResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(AgreementErrors.NotFound);
    }
}
