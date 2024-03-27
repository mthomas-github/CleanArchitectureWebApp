namespace ThirdPartyFreight.Domain.Audits;

public interface IAuditRepository
{
    Task<Audit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Audit audit);
    void Update(Audit audit);
}
