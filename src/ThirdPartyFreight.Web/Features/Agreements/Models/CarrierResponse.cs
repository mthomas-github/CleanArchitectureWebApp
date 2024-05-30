using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class CarrierResponse
{
    public Guid CarrierId { get; set; }
    public string CarrierName { get; set; }
    public string CarrierAccount { get; set; }
    public string CarrierAddress { get; set; }
    public CarrierType CarrierType { get; set; }
    
}
