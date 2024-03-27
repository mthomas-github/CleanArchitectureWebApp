using ThirdPartyFreight.Api.Controllers.Carriers;
using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Api.FunctionalTests.Carriers;

internal static class CarrierData
{
    public static AddCarrierRequest AddTestCarrierRequest =
        new(new Guid("BBEA553F-7C2F-4818-9669-650DB74DF39F"), "Unit Test", "UT1234", CarrierType.Parcel);
}