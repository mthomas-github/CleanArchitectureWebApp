using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Application.Shared;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Notes;

public class GetNoteTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/notes";

    [Fact]
    public async Task GetNote_ShouldReturnOK_WhenNoteIsRetrieved()
    {
        // Arrange
        var noteId = new Guid("2791DFB1-E1AE-4A1B-8421-6624CF9912E9");
        
        // Act
        HttpResponseMessage? response = await HttpClient.GetAsync($"{BaseUrl}/{noteId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        NoteResponse? note = await response.Content.ReadFromJsonAsync<NoteResponse>();
        note.Should().NotBeNull();
        note!.NoteId.Should().Be(noteId);
    }

    [Fact]
    public async Task GetNote_ShouldReturnNotFound_WhenNoteIsNotFound()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        
        // Act
        HttpResponseMessage? response = await HttpClient.GetAsync($"{BaseUrl}/{noteId}");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}