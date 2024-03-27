using FluentValidation;

namespace ThirdPartyFreight.Application.Notes.AddNote;

internal sealed class AddNoteValidator : AbstractValidator<AddNoteCommand>
{
    public AddNoteValidator()
    {
        RuleFor(x => x.AgreementId).NotEmpty();
        RuleFor(x => x.NoteContent).NotEmpty();
        RuleFor(x => x.NoteContent).MinimumLength(5);
        RuleFor(x => x.NoteType).NotEmpty();
    }
}
