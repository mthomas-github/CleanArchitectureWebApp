using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class EnvelopeRepository : Repository<Envelope>, IEnvelopeRepository
{
    public EnvelopeRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
