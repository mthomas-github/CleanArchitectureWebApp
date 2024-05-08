using ThirdPartyFreight.Domain.Documents;
using ThirdPartyFreight.Domain.Documents.Events;
using FluentAssertions;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Documents;

public class DocumentTest : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {

        // Act
        var document = Document.Create(DocumentData.AgreementId, DocumentData.Document);

        // Assert
        document.Id.Should().NotBeEmpty();
        document.AgreementId.Should().Be(DocumentData.AgreementId);
        document.DocumentDetails.Should().Be(DocumentData.Document);
    }

    [Fact]
    public void Create_Should_RaiseAgreementCreatedDomainEvent()
    {
        // Act
        var document = Document.Create(DocumentData.AgreementId, DocumentData.Document);

        // Assert
        DocumentCreatedDomainEvent domainEvents = AssertDomainEventWasPublished<DocumentCreatedDomainEvent>(document);

        domainEvents.DocumentId.Should().Be(document.Id);

    }
}
