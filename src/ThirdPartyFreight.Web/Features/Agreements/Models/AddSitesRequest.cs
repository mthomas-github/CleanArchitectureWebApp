namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed record AddSitesRequest(
    string AgreementId,
    List<SiteDetail> Sites);

public sealed record SiteDetail(
    string SiteNumber,
    string Street,
    string City,
    string State,
    string ZipCode);
