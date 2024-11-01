﻿using Asp.Versioning;
using ThirdPartyFreight.Application.Approvals.AddApproval;
using ThirdPartyFreight.Application.Approvals.GetApproval;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyFreight.Application.Approvals.GetApprovals;
using ThirdPartyFreight.Application.Approvals.UpdateApproval;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Api.Controllers.Approvals;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/approvals")]

public class ApprovalController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddApproval(AddApprovalRequest request, CancellationToken cancellationToken)
    {
        var command = new AddApprovalCommand(request.AgreementId);
        
        Result result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetApprovals(CancellationToken cancellationToken)
    {
        var query = new GetApprovalsQuery();

        Result<IReadOnlyList<ApprovalResponse>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetApproval(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetApprovalQuery(id);
        
        Result<ApprovalResponse> result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApproval(Guid id, UpdateApprovalRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateApprovalCommand(id, request.Approval);
        
        Result result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
