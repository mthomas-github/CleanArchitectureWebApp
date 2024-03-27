using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Envelopes.GetEnvelope;

public sealed record GetEnvelopeQuery(Guid EnvelopeId) : IQuery<EnvelopeResponse>;
