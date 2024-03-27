using ThirdPartyFreight.Domain.Carriers.Events;
using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Domain.Envelopes;
using FluentAssertions;
using ThirdPartyFreight.Domain.UnitTests.Carriers;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Envelopes;

public class EnvelopeTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {

        // Act
        var envelope = Envelope.Create(EnvelopeData.EnvelopeStatus, EnvelopeData.AgreementId, EnvelopeData.CreatedOnUtc);

        // Assert
        envelope.Id.Should().NotBeEmpty();
        envelope.AgreementId.Should().Be(EnvelopeData.AgreementId);
        envelope.EnvelopeStatus.Should().Be(EnvelopeData.EnvelopeStatus);
    }

    [Fact]
    public void Create_Should_RaiseAgreementCreatedDomainEvent()
    {
        // Act
        var carrier = Carrier.Create(CarrierData.AgreementId, CarrierData.Carrier);

        // Assert
        var domainEvents = AssertDomainEventWasPublished<CarrierCreatedDomainEvent>(carrier);

        domainEvents.CarrierId.Should().Be(carrier.Id);

    }
}