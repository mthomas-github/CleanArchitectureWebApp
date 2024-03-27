using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Sites.AddSite;

public sealed record AddSiteCommand(
    Guid AgreementId,
    string SiteNumber,
    string Street,
    string City,
    string State,
    string ZipCode) : ICommand;
