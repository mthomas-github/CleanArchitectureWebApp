using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Envelopes;

public static class EnvelopeErrors
{
    public static readonly Error NotFound = new(
                      "Envelopes.NotFound",
                                           "The envelope with the specified identifier was not found");
    public static readonly Error CannotAdd = new(
        "Envelope.NotAdded",
        "Was unable to add envelope to the specified identifier");
}
