using ThirdPartyFreight.Domain.Shared;
using ThirdPartyFreight.Domain.Sites;

namespace ThirdPartyFreight.Domain.UnitTests.Sites;

internal static class SiteData
{
    public static readonly Guid AgreementId = new("d3f3e3e3-3e3e-3e3e-3e3e-3e3e3e3e3e3e");
    public static readonly SiteNumber SiteNumber = new("1234");
    public static readonly Address SiteAddress = new("1234 Test Street", "Test City", "Test State", "12345");
}