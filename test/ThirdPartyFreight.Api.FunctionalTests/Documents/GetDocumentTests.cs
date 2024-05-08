using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Documents;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Documents;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Documents;

public class GetDocumentTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/documents";

    [Fact]
    public async Task GetDocument_ShouldReturnOK_WhenDocumentIsFound()
    {
        // Arrange
        var documentId = Guid.Parse("B2976BC8-D5CB-4827-80AD-12301909B010");

        // Act
        HttpResponseMessage? response = await HttpClient.GetAsync($"{BaseUrl}/{documentId}");

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
        DocumentResponse? document = await response.Content.ReadFromJsonAsync<DocumentResponse>();
        document.Should().NotBeNull();
        document!.DocumentId.Should().Be(documentId);
    }

    [Fact]
    public async Task GetDocument_ShouldReturnNotFound_WhenDocumentIsNotFound()
    {
        // Arrange
        var documentId = Guid.NewGuid();

        // Act
        HttpResponseMessage? response = await HttpClient.GetAsync($"{BaseUrl}/{documentId}");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}