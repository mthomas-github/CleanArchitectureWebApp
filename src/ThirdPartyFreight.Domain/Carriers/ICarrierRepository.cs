namespace ThirdPartyFreight.Domain.Carriers;

public interface ICarrierRepository
{
    Task<Carrier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Carrier carrier);
}