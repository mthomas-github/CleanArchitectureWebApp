using ThirdPartyFreight.Api.Controllers.Agreements;
using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Api.FunctionalTests.Agreements;

internal static class AgreementData
{
    public static AddAgreementRequest AddTestAgreementRequest = new(1234, "Test", "Test", "test@test.com",
        Status.Creating, AgreementType.Add, SiteType.Normal, "Test");
}