using ThirdPartyFreight.Application.Sites.AddSites;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed record AddSitesRequest(
    string AgreementId,
    List<SiteDetail> Sites);
