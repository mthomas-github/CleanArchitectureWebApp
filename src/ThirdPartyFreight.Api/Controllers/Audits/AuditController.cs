using Asp.Versioning;
using ThirdPartyFreight.Application.Audits.AddAudit;
using ThirdPartyFreight.Application.Audits.GetAudit;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.Audits;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/audits")]

public class AuditController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAudit(AddAuditRequest request, CancellationToken cancellationToken)
    {
        var command = new AddAuditCommand(request.AgreementId, request.AuditDateUtc, request.IsAuditActive);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAudit(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAuditQuery(id);

        Result<AuditResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
