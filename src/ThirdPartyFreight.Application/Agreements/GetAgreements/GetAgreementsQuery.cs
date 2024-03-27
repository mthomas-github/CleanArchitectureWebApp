using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Agreements.GetAgreements;

public sealed record GetAgreementsQuery() : IQuery<IReadOnlyList<AgreementResponse>>;
