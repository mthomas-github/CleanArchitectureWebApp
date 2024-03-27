using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Infrastructure.Repositories;

internal sealed class NoteRepository : Repository<Note> , INoteRepository
{
    public NoteRepository(ApplicationDbContext dbContext) 
        : base(dbContext)
    {
    }
}
