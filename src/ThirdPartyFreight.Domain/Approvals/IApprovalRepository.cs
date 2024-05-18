namespace ThirdPartyFreight.Domain.Approvals;

public interface IApprovalRepository
{
    Task<Approval?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Approval approval);
    void Update(Approval approval);

    Task<Approval?> GetByTaskIdAsync(string taskId, CancellationToken cancellationToken = default);
    
    Task<Approval?> GetByAgreementIdAsync(Guid AgreementId, CancellationToken cancellationToken = default);
}
