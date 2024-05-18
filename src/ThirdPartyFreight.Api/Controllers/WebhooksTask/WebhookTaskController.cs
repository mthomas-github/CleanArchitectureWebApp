using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using ThirdPartyFreight.Application.Abstractions.Elsa;
using ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;
using ThirdPartyFreight.Application.WorkflowTasks.GetWorkFlowTask;
using ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/webhooktasks")]
public class WebhookTaskController(ISender sender, IElsaService elsaService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RunTask(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var command = new AddWorkFlowTaskCommand(
            webhookEvent.Payload.TaskId,
            webhookEvent.Payload.WorkflowInstanceId,
            webhookEvent.Payload.TaskName,
            webhookEvent.Payload.TaskPayload.Description,
            Guid.Parse(webhookEvent.Payload.TaskPayload.Approval.AgreementId));

        Result<Guid> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTask(Guid taskId, CancellationToken cancellationToken)
    {
        var query = new GetWorkflowTask(taskId);

        Result<WorkFlowTask> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPut("{taskId:guid}")]
    public async Task<IActionResult> CompleteTask(Guid taskId, WebHookTaskRequest webHookTaskRequest,
        CancellationToken cancellationToken)
    {
        await elsaService.CompleteTask(webHookTaskRequest.WorkFlowTask.ExternalId,
            cancellationToken: cancellationToken);
        
        var command = new UpdateWorkFlowTaskCommand(taskId, webHookTaskRequest.WorkFlowTask);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
