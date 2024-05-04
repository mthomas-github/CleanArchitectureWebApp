using ThirdPartyFreight.Application.Sites.AddSites;

namespace ThirdPartyFreight.Api.Controllers.Sites;

public sealed record AddSitesRequest(
    Guid AgreementId,
    SiteDetail[] Sites);



