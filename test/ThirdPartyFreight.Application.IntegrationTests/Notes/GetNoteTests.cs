using ThirdPartyFreight.Application.Notes.GetNote;
using ThirdPartyFreight.Domain.Notes;
using FluentAssertions;
using ThirdPartyFreight.Application.IntegrationTests.Infrastructure;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.IntegrationTests.Notes;

public class GetNoteTests : BaseIntegrationTest
{
    private static readonly Guid NoteId = Guid.NewGuid();
    public GetNoteTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetNote_ShouldReturnFailure_WhenAgreementIsNotFound()
    {
        // Arrange
        var query = new GetNoteQuery(NoteId);

        // Act
        Result<NoteResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(NoteErrors.NotFound);
        ;
    }
}
