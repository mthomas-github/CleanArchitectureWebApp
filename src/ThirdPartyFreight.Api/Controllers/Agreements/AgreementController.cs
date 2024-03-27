using Asp.Versioning;
using ThirdPartyFreight.Application.Agreements.AddAgreement;
using ThirdPartyFreight.Application.Agreements.GetAgreement;
using ThirdPartyFreight.Application.Agreements.GetAgreements;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.Agreements;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/agreements")]
public class AgreementController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAgreements(CancellationToken cancellationToken)
    {
        var query = new GetAgreementsQuery();

        Result<IReadOnlyList<AgreementResponse>> result = await sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAgreement(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAgreementQuery(id);

        Result<AgreementResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();

    }

    [HttpPost]
    public async Task<IActionResult> AddAgreement(AddAgreementRequest request, CancellationToken cancellationToken)
    {
        var command = new AddAgreementCommand(
                       request.CustomerNumber,
                                  request.CustomerName,
                                  request.ContactName,
                                  request.ContactEmail,
                                  request.Status,
                                  request.AgreementType,
                                  request.SiteType,
                                  new CreatedBy(request.CreatedBy));

        Result<Guid> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetAgreement), new { id = result.Value }, result.Value);
    }
}
