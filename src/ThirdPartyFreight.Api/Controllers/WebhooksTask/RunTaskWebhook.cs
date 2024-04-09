namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public sealed record RunTaskWebhook(string WorkflowInstanceId, string TaskId, string TaskName, TaskPayload TaskPayload);
