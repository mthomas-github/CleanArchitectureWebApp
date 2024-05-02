namespace ThirdPartyFreight.Domain.Carriers;

public record CarrierInfo(
    string CarrierName,
    string CarrierAccount,
    string CarrierAddress,
    CarrierType CarrierType);
