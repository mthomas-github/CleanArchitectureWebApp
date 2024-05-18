namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public sealed record CompleteTaskRequest(Guid WorkFlowTaskId, string TaskId);
