using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Audits;

public static class AuditErrors
{
    public static readonly Error NotFound = new(
        "Audit.NotFound",
        "The audit with the specified identifier was not found");

    public static readonly Error CannotAdd = new(
        "Audit.NotAdded",
        "Was unable to add audit to the specified identifier");
}
