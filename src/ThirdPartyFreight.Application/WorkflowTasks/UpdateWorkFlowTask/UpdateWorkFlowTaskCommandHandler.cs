using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

internal sealed class UpdateWorkFlowTaskCommandHandler(IWorkFlowTaskRepository workFlowTaskRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateWorkFlowTaskCommand>
{
    public async Task<Result> Handle(UpdateWorkFlowTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            WorkFlowTask? existingWorkFlowTask =
                await workFlowTaskRepository.GetByIdAsync(request.WorkFlowTaskId, cancellationToken);

            if (existingWorkFlowTask is null)
            {
                return Result.Failure(WorkFlowTaskErrors.NotFound);
            }

            var updated = WorkFlowTask.Complete(existingWorkFlowTask);

            workFlowTaskRepository.Update(updated);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(WorkFlowTaskErrors.CannotUpdate);
        }
    }
}
