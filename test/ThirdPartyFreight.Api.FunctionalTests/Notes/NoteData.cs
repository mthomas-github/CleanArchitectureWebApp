using ThirdPartyFreight.Api.Controllers.Notes;
using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Api.FunctionalTests.Notes;

internal static class NoteData
{
    public static readonly AddNoteRequest AddNoteRequest = new(new Guid("BBEA553F-7C2F-4818-9669-650DB74DF39F"),
        "This is my test note", NoteType.TeamResponse);

}