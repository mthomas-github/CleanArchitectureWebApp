using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/webhooktasks")]

public class WebhookTaskController : ControllerBase
{
    [HttpPost("run-task")]
    public async Task<IActionResult> RunTask(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {

    }
}
