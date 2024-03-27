using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Notes;

public static class NoteErrors
{
    public static readonly Error NotFound = new(
               "Note.NotFound",
                      "The note with the specified identifier was not found");
    public static readonly Error CannotAdd = new(
        "Note.NotAdded",
        "Was unable to add note to the specified identifier");
}
