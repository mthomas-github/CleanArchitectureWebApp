using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Approvals;

public static class ApprovalErrors
{
    public static readonly Error NotFound = new(
        "Approval.NotFound",
        "The approval with the specified identifier was not found");

    public static readonly Error CannotAdd = new(
        "Approval.NotAdded",
        "Was unable to add approval to the specified identifier");

    public static readonly Error CannotUpdate = new(
        "Approval.CannotUpdate",
        "Was unable to update approval to the specified identifier");
}
