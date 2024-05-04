using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed record AddEnvelopeRequest(EnvelopeStatus EnvelopeStatus, string AgreementId);
