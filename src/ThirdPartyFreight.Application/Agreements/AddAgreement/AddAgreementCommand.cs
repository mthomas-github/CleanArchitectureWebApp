using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

public sealed record AddAgreementCommand(
    int CustomerNumber,
    string CustomerName,
    string ContactName,
    string ContactEmail,
    Status Status,
    AgreementType AgreementType,
    SiteType SiteType,
    CreatedBy CreatedBy) : ICommand<Guid>;
