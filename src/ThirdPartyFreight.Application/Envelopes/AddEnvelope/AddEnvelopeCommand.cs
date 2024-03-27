using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Envelopes.AddEnvelope;

public sealed record AddEnvelopeCommand(
    Guid AgreementId,
    EnvelopeStatus EnvelopeStatus) : ICommand;
