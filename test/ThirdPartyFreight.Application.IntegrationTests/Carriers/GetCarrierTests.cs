using ThirdPartyFreight.Application.Carriers.GetCarrier;
using ThirdPartyFreight.Domain.Carriers;
using FluentAssertions;
using ThirdPartyFreight.Application.IntegrationTests.Infrastructure;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.IntegrationTests.Carriers;

public class GetCarrierTests : BaseIntegrationTest
{
    private static readonly Guid CarrierId = Guid.NewGuid();
    public GetCarrierTests(IntegrationTestWebAppFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task GetCarrier_ShouldReturnFailure_WhenAgreementIsNotFound()
    {
        // Arrange
        var query = new GetCarrierQuery(CarrierId);

        // Act
        Result<CarrierResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(CarrierErrors.NotFound);
        ;        }
}
