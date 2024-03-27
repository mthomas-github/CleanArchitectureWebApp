namespace ThirdPartyFreight.Api.Controllers.Sites;

public sealed record AddSiteRequest(
    Guid AgreementId,
    string SiteNumber,
    string Street,
    string City,
    string State,
    string ZipCode);
