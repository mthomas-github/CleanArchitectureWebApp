using ThirdPartyFreight.Domain.Notes;
using ThirdPartyFreight.Domain.Notes.Events;
using FluentAssertions;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Notes;

public class NoteTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {

        // Act
        var note = Note.Create(NoteData.AgreementId, NoteData.NoteContent, DateTime.UtcNow, NoteData.NoteType);

        // Assert
        note.Id.Should().NotBeEmpty();
        note.AgreementId.Should().Be(NoteData.AgreementId);
        note.NoteType.Should().Be(NoteData.NoteType);
    }

    [Fact]
    public void Create_Should_RaiseAgreementCreatedDomainEvent()
    {
        // Act
        var note = Note.Create(NoteData.AgreementId, NoteData.NoteContent, DateTime.UtcNow, NoteData.NoteType);

        // Assert
        var domainEvents = AssertDomainEventWasPublished< NoteCreatedDomainEvent>(note);

        domainEvents.NoteId.Should().Be(note.Id);

    }
}