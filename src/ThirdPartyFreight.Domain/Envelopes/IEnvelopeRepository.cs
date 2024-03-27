
namespace ThirdPartyFreight.Domain.Envelopes;

public interface IEnvelopeRepository
{
    Task<Envelope?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Envelope envelope);
}
