
namespace ThirdPartyFreight.Domain.Notes;

public interface INoteRepository
{
    Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Note note);
}
