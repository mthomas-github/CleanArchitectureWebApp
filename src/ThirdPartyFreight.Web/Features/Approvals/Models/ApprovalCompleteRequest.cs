namespace ThirdPartyFreight.Web.Features.Approvals.Models;

public sealed record ApprovalCompleteRequest(Guid WorkFlowTaskId, string TaskId);
