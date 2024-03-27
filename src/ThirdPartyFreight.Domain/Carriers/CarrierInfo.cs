namespace ThirdPartyFreight.Domain.Carriers;

public record CarrierInfo(
    string CarrierName,
    string CarrierAccount,
    CarrierType CarrierType);