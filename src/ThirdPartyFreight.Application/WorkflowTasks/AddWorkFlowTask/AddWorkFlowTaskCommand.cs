using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;

public sealed record AddWorkFlowTaskCommand(
    string ExternalId,
    string ProcessId,
    string Name,
    string Description,
    Guid AgreementId) : ICommand<Guid>;
