using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

public sealed record UpdateWorkFlowTaskCommand(Guid WebHookTaskId) : ICommand;
