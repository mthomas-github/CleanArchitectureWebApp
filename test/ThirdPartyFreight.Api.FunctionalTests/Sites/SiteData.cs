using ThirdPartyFreight.Api.Controllers.Sites;

namespace ThirdPartyFreight.Api.FunctionalTests.Sites;

internal static class SiteData
{
    public static readonly AddSiteRequest AddSiteRequest = new(new Guid("BBEA553F-7C2F-4818-9669-650DB74DF39F"),
        "99999", "999 Elm St", "Elm City", "CA", "55588");
}