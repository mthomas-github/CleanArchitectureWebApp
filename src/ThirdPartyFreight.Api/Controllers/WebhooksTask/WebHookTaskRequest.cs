using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;
public sealed record WebHookTaskRequest(Guid WebHookTaskId, WorkFlowTask WorkFlowTask);

