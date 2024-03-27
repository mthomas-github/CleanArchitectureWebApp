using ThirdPartyFreight.Application.Documents.GetDocument;
using ThirdPartyFreight.Domain.Documents;
using FluentAssertions;
using ThirdPartyFreight.Application.IntegrationTests.Infrastructure;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.IntegrationTests.Documents;

public class GetDocumentTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly Guid DocumentId = Guid.NewGuid();


    [Fact]
    public async Task GetDocument_ShouldReturnFailure_WhenAgreementIsNotFound()
    {
        // Arrange
        var query = new GetDocumentQuery(DocumentId);

        // Act
        Result<DocumentResponse> result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(DocumentErrors.NotFound);
    }
}
