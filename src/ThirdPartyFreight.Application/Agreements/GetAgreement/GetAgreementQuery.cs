using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Agreements.GetAgreement;

public sealed record GetAgreementQuery(Guid AgreementId) : IQuery<AgreementResponse>;
