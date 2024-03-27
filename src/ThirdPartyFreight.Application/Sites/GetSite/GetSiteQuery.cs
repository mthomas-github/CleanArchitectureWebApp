using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Sites.GetSite;

public sealed record GetSiteQuery(Guid SiteId) : IQuery<SiteResponse>;
