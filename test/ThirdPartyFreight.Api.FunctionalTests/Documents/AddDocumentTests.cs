using System.Net;
using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Documents;
using ThirdPartyFreight.Domain.Documents;
using FluentAssertions;
using ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

namespace ThirdPartyFreight.Api.FunctionalTests.Documents;

public class AddDocumentTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/documents";
    private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";

    [Theory]
    [InlineData("9b5c9c25-21d5-4b73-9ab9-d487d0f4f69a", "TestDocument", "", DocumentType.Agreement)]
    [InlineData("d5f43aee-7dd1-47db-98b1-0ee4bfcb9d05", "", "", DocumentType.RoutingGuide)]
    [InlineData(EmptyGuid, "", "TestData", DocumentType.UserProvided)]
    [InlineData("d78c0624-4f9e-4f3e-aa4f-7e49f93bbbd7", "TestDocument", "YUP", (DocumentType)558)]
    [InlineData("6d02c695-151f-439a-8c4d-d29a122b2c2a", "TestDocument", "", (DocumentType)100)]

    public async Task AddDocument_ShouldReturnBadRequest_WhenDocumentRequestIsInvalid(
        string agreementId,
        string documentName,
        string documentData,
        DocumentType documentType)
    {
        // Arrange
        var addDocumentRequest = new AddDocumentRequest(Guid.Parse(agreementId), documentName, documentData, documentType);

        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, addDocumentRequest);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddDocument_ShouldReturnOK_WhenDocumentIsAdded()
    {
        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, DocumentData.AddTestDocumentRequest);

        // Assert
        response.Should().NotBeNull();
        response.EnsureSuccessStatusCode();
    }
}