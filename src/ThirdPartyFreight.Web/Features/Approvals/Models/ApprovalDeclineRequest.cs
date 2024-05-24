namespace ThirdPartyFreight.Web.Features.Approvals.Models;

public sealed record ApprovalDeclineRequest(Guid ApprovalId, Guid ProcessId);
