using ThirdPartyFreight.Api.Controllers.Envelopes;
using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Api.FunctionalTests.Envelopes;

internal static class EnvelopeData
{
    public static AddEnvelopeRequest AddTestEnvelopeRequest = new(EnvelopeStatus.Draft, new Guid("BBEA553F-7C2F-4818-9669-650DB74DF39F"));
}