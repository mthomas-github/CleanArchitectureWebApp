using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Notes;
using ThirdPartyFreight.Domain.Notes;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Notes;

public class AddNoteTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/notes";
    private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";

    [Fact]
    public async Task AddNote_ShouldReturnOK_WhenNoteIsAdded()
    {
        // Act
        HttpResponseMessage? response = await HttpClient.PostAsJsonAsync(BaseUrl, NoteData.AddNoteRequest);

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData(EmptyGuid, "", NoteType.AutoResponse)]
    [InlineData("d78c0624-4f9e-4f3e-aa4f-7e49f93bbbd7", "12", NoteType.AutoResponse)]
    [InlineData(EmptyGuid, "Test4423", (NoteType)20)]
    public async Task AddNote_ShouldReturnBadRequest_WhenNoteRequestIsInvalid(
        string agreementId,
        string noteContent,
        NoteType noteType)
    {
        // Arrange
        var addNoteRequest = new AddNoteRequest(new Guid(agreementId), noteContent, noteType);

        // Act
        HttpResponseMessage? response = await HttpClient.PostAsJsonAsync(BaseUrl, addNoteRequest);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}