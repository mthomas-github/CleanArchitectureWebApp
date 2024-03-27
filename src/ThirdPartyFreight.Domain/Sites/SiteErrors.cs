using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Sites;

public static class SiteErrors
{
    public static readonly Error NotFound = new(
        "Site.NotFound",
        "The site with the specified identifier was not found");

    public static readonly Error CannotAdd = new(
        "Site.CannotAdd",
        "The was unable to be added.");
}
