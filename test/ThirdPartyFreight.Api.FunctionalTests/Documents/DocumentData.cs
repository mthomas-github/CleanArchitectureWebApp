using ThirdPartyFreight.Api.Controllers.Documents;
using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Api.FunctionalTests.Documents;

internal static class DocumentData
{
    public static AddDocumentRequest AddTestDocumentRequest => new(
        new Guid("BBEA553F-7C2F-4818-9669-650DB74DF39F"), "My Unit Test Doc Name", "WOW", DocumentType.Agreement);
}