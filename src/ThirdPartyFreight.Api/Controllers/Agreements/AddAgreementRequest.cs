using ThirdPartyFreight.Domain.Agreements;

namespace ThirdPartyFreight.Api.Controllers.Agreements;

public sealed record AddAgreementRequest(
    int CustomerNumber,
    string CustomerName,
    string ContactName,
    string ContactEmail,
    Status Status,
    AgreementType AgreementType,
    SiteType SiteType,
    string CreatedBy);
