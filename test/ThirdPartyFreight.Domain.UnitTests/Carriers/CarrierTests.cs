using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Domain.Carriers.Events;
using FluentAssertions;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Carriers;

public class CarrierTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {

        // Act
        var carrier = Carrier.Create(CarrierData.AgreementId, CarrierData.Carrier);

        // Assert
        carrier.Id.Should().NotBeEmpty();
        carrier.AgreementId.Should().Be(CarrierData.AgreementId);
        carrier.CarrierInfo.Should().Be(CarrierData.Carrier);
    }

    [Fact]
    public void Create_Should_RaiseAgreementCreatedDomainEvent()
    {
        // Act
        var carrier = Carrier.Create(CarrierData.AgreementId, CarrierData.Carrier);

        // Assert
        CarrierCreatedDomainEvent domainEvents = AssertDomainEventWasPublished<CarrierCreatedDomainEvent>(carrier);

        domainEvents.CarrierId.Should().Be(carrier.Id);

    }
}
