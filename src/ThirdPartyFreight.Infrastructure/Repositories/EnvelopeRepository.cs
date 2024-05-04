using Microsoft.EntityFrameworkCore;
using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class EnvelopeRepository : Repository<Envelope>, IEnvelopeRepository
{
    public EnvelopeRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }

    public void Update(Envelope envelope)
    {
        DbContext.Set<Envelope>().Update(envelope);
    }

    public async Task<Envelope> GetEnvelopeAsyncByAgreementId(Guid agreementId, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Envelope>()
            .FirstOrDefaultAsync(envelope => envelope.AgreementId == agreementId, cancellationToken);
    }
}
