using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Domain.WorkflowTask.Events;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

public class UpdateWorkFlowTaskDomainEventHandler(ILogger<UpdateWorkFlowTaskDomainEventHandler> logger) : INotificationHandler<WorkFlowTaskUpdatedDomainEvent>
{
    public Task Handle(WorkFlowTaskUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogCritical("Running UpdateWorkFlowTaskDomainEventHandler");
        
        return Task.CompletedTask;
    }
}
