using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Application.Shared;

public sealed class CarrierResponse
{
    public Guid CarrierId { get; init; }
    public string CarrierName { get; init; }
    public string CarrierAccount { get; init; }
    public string CarrierAddress { get; init; }
    public CarrierType CarrierType { get; init; }
}
