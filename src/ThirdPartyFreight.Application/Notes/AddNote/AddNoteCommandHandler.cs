using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Notes;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Notes.AddNote;

internal sealed class AddNoteCommandHandler(
    INoteRepository noteRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddNoteCommand>
{
    public async Task<Result> Handle(AddNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var note = Note.Create(request.AgreementId, new Content(request.NoteContent), dateTimeProvider.UtcNow,
                request.NoteType);

            noteRepository.Add(note);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(NoteErrors.CannotAdd);
        }
    }
}
