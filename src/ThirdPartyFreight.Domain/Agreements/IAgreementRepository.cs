namespace ThirdPartyFreight.Domain.Agreements;

public interface IAgreementRepository
{
    Task<Agreement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Agreement agreement);

}
