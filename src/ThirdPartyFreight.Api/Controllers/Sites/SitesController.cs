using Asp.Versioning;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Application.Sites.AddSite;
using ThirdPartyFreight.Application.Sites.GetSite;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyFreight.Application.Sites.AddSites;

namespace ThirdPartyFreight.Api.Controllers.Sites;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/sites")]

public class SitesController(ISender sender) : ControllerBase
{
    [HttpGet("{siteId}")]
    public async Task<ActionResult> GetSite(Guid siteId, CancellationToken cancellationToken)
    {
        var query = new GetSiteQuery(siteId);

        Result<SiteResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);

    }

    [HttpPost("AddSite")]
    public async Task<IActionResult> AddSite(AddSiteRequest request, CancellationToken cancellationToken)
    {
        var command = new AddSiteCommand(request.AgreementId, request.SiteNumber, request.Street, request.City, request.State, request.ZipCode);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPost("AddSites")]
    public async Task<IActionResult> AddSites(AddSitesRequest request, CancellationToken cancellationToken)
    {
        var command = new AddSitesCommand(request.AgreementId, request.Sites);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
