namespace ThirdPartyFreight.Domain.Agreements;

public record ContactInfo(
    int CustomerNumber,
    string CompanyName,
    string CustomerName,
    string CustomerEmail);