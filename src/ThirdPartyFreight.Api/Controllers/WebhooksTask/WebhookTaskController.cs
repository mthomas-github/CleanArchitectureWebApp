using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;
using ThirdPartyFreight.Application.WorkflowTasks.GetWorkFlowTask;
using ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/webhooks")]

public class WebhookTaskController(ISender sender) : ControllerBase
{
    [HttpPost("run-task")]
    public async Task<IActionResult> RunTask(WebhookEvent webhook, CancellationToken cancellationToken)
    {
        RunTaskWebhook payload = webhook.Payload;
        TaskPayload taskPayload = payload.TaskPayload;

        var task = new WebHookTaskRequest
        {
            ProcessId = payload.WorkflowInstanceId,
            ExternalId = payload.TaskId,
            Name = payload.TaskName,
            Description = taskPayload.Description,
            AgreementId = taskPayload.AgreementId,
            CreatedAt = DateTimeOffset.Now
        };

        var command = new AddWorkFlowTaskCommand(
                       task.ExternalId,
                                  task.ProcessId,
                                  task.Name,
                                  task.Description,
                                  task.AgreementId);

        Result<Guid> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> CompleteTask(Guid taskId, WebHookTaskRequest webhook, CancellationToken cancellationToken)
    {
        var command = new UpdateWorkFlowTaskCommand(taskId, WorkFlowTask.Update(webhook.Id, webhook.ExternalId, webhook.ProcessId, webhook.Name, webhook.Description, webhook.AgreementId));
        
        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTask(Guid taskId, CancellationToken cancellationToken)
    {
        var query = new GetWorkflowTask(taskId);

        Result<WorkFlowTask> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
