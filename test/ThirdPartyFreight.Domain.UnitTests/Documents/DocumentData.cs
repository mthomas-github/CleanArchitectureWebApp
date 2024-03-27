using ThirdPartyFreight.Domain.Documents;

namespace ThirdPartyFreight.Domain.UnitTests.Documents;

internal static class DocumentData
{
    public static readonly Guid AgreementId = new("d3f3e3e3-3e3e-3e3e-3e3e-3e3e3e3e3e3e");
    public static readonly Details Document = new("Test Document", "1234", DocumentType.Agreement);
}