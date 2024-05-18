using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;

public sealed record AddWorkFlowTaskCommand(
    string ExternalId,
    string ProcessId,
    string Name,
    ApproverType Approver,
    Guid AgreementId) : ICommand<Guid>;
