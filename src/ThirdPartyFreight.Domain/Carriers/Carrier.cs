using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Carriers.Events;

namespace ThirdPartyFreight.Domain.Carriers;

public sealed class Carrier : Entity
{
    private Carrier(
        Guid id,
        Guid agreementId,
        CarrierInfo carrierInfo)
        : base(id)
    {
        AgreementId = agreementId;
        CarrierInfo = carrierInfo;
    }
    private Carrier() { }
    public Guid AgreementId { get; private set; }
    public CarrierInfo CarrierInfo { get; private set; }

    public static Carrier Create(
        Guid agreementId,
        CarrierInfo carrierInfo)
    {
        var carrier = new Carrier(
                       Guid.NewGuid(),
                                  agreementId,
                                  carrierInfo);

        carrier.RaiseDomainEvent(new CarrierCreatedDomainEvent(carrier.Id));
        return carrier;
    }
}
