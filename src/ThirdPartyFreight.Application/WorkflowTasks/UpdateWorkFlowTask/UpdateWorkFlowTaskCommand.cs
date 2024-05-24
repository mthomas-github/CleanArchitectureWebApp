using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

public sealed record UpdateWorkFlowTaskCommand(Guid WebHookTaskId, bool Voided) : ICommand;
