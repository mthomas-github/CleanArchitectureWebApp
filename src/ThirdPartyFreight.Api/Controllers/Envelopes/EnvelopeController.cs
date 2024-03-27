using Asp.Versioning;
using ThirdPartyFreight.Application.Envelopes.AddEnvelope;
using ThirdPartyFreight.Application.Envelopes.GetEnvelope;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.Envelopes;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/envelopes")]

public class EnvelopeController(ISender sender) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEnvelope(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetEnvelopeQuery(id);

        Result<EnvelopeResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> AddEnvelope(AddEnvelopeRequest request, CancellationToken cancellationToken)
    {
        var command = new AddEnvelopeCommand(request.AgreementId, request.EnvelopeStatus);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
