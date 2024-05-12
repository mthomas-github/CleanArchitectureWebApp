namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public abstract record Payload(string WorkflowInstanceId, string WorkflowDefinitionId, string WorkflowName, object? CorrelationId, string TaskId, string TaskName, TaskPayload TaskPayload);
