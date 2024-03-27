using Asp.Versioning;
using ThirdPartyFreight.Application.Notes.AddNote;
using ThirdPartyFreight.Application.Notes.GetNote;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyFreight.Api.Controllers.Notes;
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/notes")]

public class NotesController(ISender sender) : ControllerBase
{
    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNote(Guid noteId, CancellationToken cancellationToken)
    {
        var query = new GetNoteQuery(noteId);

        Result<NoteResponse> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> AddNote(AddNoteRequest request, CancellationToken cancellationToken)
    {
        var command = new AddNoteCommand(request.AgreementId, request.NoteContent, request.NoteType);

        Result result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

}
