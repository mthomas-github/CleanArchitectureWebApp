using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Api.Controllers.Envelopes;

public sealed record AddEnvelopeRequest(EnvelopeStatus EnvelopeStatus, Guid AgreementId);
