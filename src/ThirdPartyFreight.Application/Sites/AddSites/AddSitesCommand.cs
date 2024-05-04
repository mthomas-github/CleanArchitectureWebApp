
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Sites.AddSites;

public sealed record AddSitesCommand(
    Guid AgreementId,
    SiteDetail[] Sites) : ICommand;
public sealed record SiteDetail(
    string SiteNumber,
    string Street,
    string City,
    string State,
    string ZipCode);
