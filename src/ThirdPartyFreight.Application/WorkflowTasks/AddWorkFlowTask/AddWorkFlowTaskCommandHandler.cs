﻿using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;

internal sealed class AddWorkFlowTaskCommandHandler(
    IWorkFlowTaskRepository workFlowTaskRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddWorkFlowTaskCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddWorkFlowTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var workFlowTask = WorkFlowTask.Create(request.ExternalId, request.ProcessId, request.Name,
                request.Approver, request.AgreementId, dateTimeProvider.UtcNow);

            workFlowTaskRepository.Add(workFlowTask);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return workFlowTask.Id;
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(WorkFlowTaskErrors.CannotAdd);
        }
    }
}
