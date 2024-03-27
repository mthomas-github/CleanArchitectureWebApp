using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Agreements.Events;
using FluentAssertions;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Agreements;

public class AgreementTest : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Arrange See UserData.cs

        // Act
        var agreement = Agreement.Create(AgreementData.ContactInfo, AgreementData.Status, AgreementData.AgreementType, AgreementData.SiteType, AgreementData.CreatedBy, AgreementData.CreatedDate);

        // Assert
        agreement.ContactInfo.Should().Be(AgreementData.ContactInfo);
        agreement.Status.Should().Be(AgreementData.Status);
        agreement.AgreementType.Should().Be(AgreementData.AgreementType);
        agreement.SiteType.Should().Be(AgreementData.SiteType);
        agreement.CreatedBy.Should().Be(AgreementData.CreatedBy);
        agreement.CreatedOnUtc.Should().Be(AgreementData.CreatedDate);
    }

    [Fact]
    public void Create_Should_RaiseAgreementCreatedDomainEvent()
    {
        // Act
        var agreement = Agreement.Create(AgreementData.ContactInfo, AgreementData.Status, AgreementData.AgreementType, AgreementData.SiteType, AgreementData.CreatedBy, AgreementData.CreatedDate);

        // Assert
        AgreementCreatedDomainEvent domainEvents = AssertDomainEventWasPublished<AgreementCreatedDomainEvent>(agreement);

        domainEvents.AgreementId.Should().Be(agreement.Id);

    }

    [Fact]
    public void Complete_WhenStatusIsNotCompleted_ReturnsFailureResult()
    {
        // Act
        var agreement = Agreement.Create(AgreementData.ContactInfo, AgreementData.Status, AgreementData.AgreementType, AgreementData.SiteType, AgreementData.CreatedBy, AgreementData.CreatedDate);
        Result result = agreement.Complete(DateTime.UtcNow, new ModifiedBy("TestName"));
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AgreementErrors.NotComplete);
        agreement.Status.Should().Be(Status.Creating);

    }

    [Fact]
    public void Complete_WhenStatusIsCompleted_ReturnsSuccessResult_And_UpdatesStatus_ModifiedOnUtc_ModifiedBy()
    {
        // Act
        var agreement = Agreement.Create(AgreementData.ContactInfo, Status.Completed, AgreementData.AgreementType, AgreementData.SiteType, AgreementData.CreatedBy, AgreementData.CreatedDate);
        DateTime utcNow = DateTime.UtcNow;
        var modifiedBy = new ModifiedBy("TestName");
        Result result = agreement.Complete(utcNow, modifiedBy);


        // Assert
        result.IsSuccess.Should().BeTrue();
        agreement.Status.Should().Be(Status.Completed);
        agreement.ModifiedOnUtc.Should().Be(utcNow);
        agreement.ModifiedBy.Should().Be(modifiedBy);
    }

    [Fact]
    public void Complete_Method_Updates_Status_And_Modified_Details()
    {
        // Arrange
        DateTime utcNow = DateTime.UtcNow;
        var modifiedBy = new ModifiedBy("userId");
        var agreement = Agreement.Create(
            new ContactInfo(1234, "Doe","Joe", "john@example.com"),
            Status.Completed,
            AgreementType.Add,
            SiteType.Normal,
            new CreatedBy("creatorId"),
            utcNow);

        // Act
        Result result = agreement.Complete(utcNow, modifiedBy);

        // Assert
        result.IsSuccess.Should().BeTrue();
        agreement.Status.Should().Be(Status.Completed);
        agreement.ModifiedOnUtc.Should().Be(utcNow);
        agreement.ModifiedBy.Should().Be(modifiedBy);
    }

    [Fact]
    public void Complete_Method_With_FluentValidation_Raises_DomainEvent_And_Returns_Success()
    {
        // Arrange
        var agreement = Agreement.Create(AgreementData.ContactInfo, Status.Completed, AgreementData.AgreementType, AgreementData.SiteType, AgreementData.CreatedBy, AgreementData.CreatedDate);

        agreement.Complete(DateTime.UtcNow, new ModifiedBy("TestName"));

        AgreementCompletedDomainEvent domainEvents = AssertDomainEventWasPublished<AgreementCompletedDomainEvent>(agreement);

        domainEvents.AgreementId.Should().Be(agreement.Id);
    }
}
