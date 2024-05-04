
using ThirdPartyFreight.Domain.Sites;

namespace ThirdPartyFreight.Domain.Envelopes;

public interface IEnvelopeRepository
{
    Task<Envelope?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Envelope envelope);

    void Update(Envelope envelope);

    Task<Envelope> GetEnvelopeAsyncByAgreementId(
        Guid agreementId,
        CancellationToken cancellationToken = default);
}
