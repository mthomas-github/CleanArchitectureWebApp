namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public record ApprovalDeclineRequest(Guid ApprovalId, Guid WorkFlowTaskId, string ProcessId);
