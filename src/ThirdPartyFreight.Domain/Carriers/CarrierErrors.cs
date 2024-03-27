using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Carriers;

public static class CarrierErrors
{
    public static readonly Error NotFound = new(
               "Carrier.NotFound",
                      "The carrier with the specified identifier was not found");

    public static readonly Error CannotAdd = new(
        "Carrier.NotAdded",
        "Was unable to add carrier to the specified identifier");
}
