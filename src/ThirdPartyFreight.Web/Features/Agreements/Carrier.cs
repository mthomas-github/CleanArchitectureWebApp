using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Web.Features.Agreements;

public class Carrier
{
    public Guid AgreementId { get; set; }
    public string CarrierName { get; set; }
    public string CarrierAccount { get; set; }
    public string? CarrierAddress { get; set; }
    public CarrierType CarrierType { get; set; }
}
