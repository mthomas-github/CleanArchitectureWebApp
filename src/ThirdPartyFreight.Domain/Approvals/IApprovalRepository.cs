namespace ThirdPartyFreight.Domain.Approvals;

public interface IApprovalRepository
{
    Task<Approval?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Approval approval);
    void Update(Approval approval);
}
