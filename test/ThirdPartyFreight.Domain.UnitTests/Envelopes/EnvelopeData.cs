using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Domain.UnitTests.Envelopes;

internal static class EnvelopeData
{
    public static readonly EnvelopeStatus EnvelopeStatus = EnvelopeStatus.Draft;
    public static readonly Guid AgreementId = new("d3f3e3e3-3e3e-3e3e-3e3e-3e3e3e3e3e3e");
    public static readonly DateTime CreatedOnUtc = DateTime.UtcNow;
}