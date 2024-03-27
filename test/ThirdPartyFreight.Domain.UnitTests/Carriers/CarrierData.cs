using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Domain.UnitTests.Carriers;

internal static class CarrierData
{
    public static readonly Guid AgreementId = new("d3f3e3e3-3e3e-3e3e-3e3e-3e3e3e3e3e3e");
    public static readonly CarrierInfo Carrier = new("Test Carrier", "1234", CarrierType.Parcel);
}