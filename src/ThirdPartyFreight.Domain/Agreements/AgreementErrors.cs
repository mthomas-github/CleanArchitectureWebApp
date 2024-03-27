using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Agreements;

public static class AgreementErrors
{
    public static readonly Error NotComplete = new(
        "Agreement.NotComplete",
        "The agreement with the specified identifier is not complete.");

    public static readonly Error NotFound = new(
        "Agreement.NotComplete",
        "The agreement with the specified identifier is not found.");
}
