using Asp.Versioning;
using ThirdPartyFreight.Application.Documents.AddDocument;
using ThirdPartyFreight.Application.Documents.GetDocument;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.Documents;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/documents")]

public class DocumentController(ISender sender) : ControllerBase
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetDocument(Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetDocumentQuery(Id);

        Result<DocumentResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> AddDocument(AddDocumentRequest request, CancellationToken cancellationToken)
    {
        var command = new AddDocumentCommand(request.AgreementId, request.DocumentName, request.DocumentData, request.DocumentType);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
