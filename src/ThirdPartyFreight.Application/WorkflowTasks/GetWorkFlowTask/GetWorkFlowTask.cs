﻿using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.GetWorkFlowTask;

public record GetWorkflowTask(Guid WorkflowTaskId) : IQuery<WorkFlowTask>;
