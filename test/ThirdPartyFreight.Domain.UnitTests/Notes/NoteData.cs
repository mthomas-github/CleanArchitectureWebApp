using ThirdPartyFreight.Domain.Notes;

namespace ThirdPartyFreight.Domain.UnitTests.Notes;

internal static class NoteData
{
    public static readonly Guid AgreementId = new("d3f3e3e3-3e3e-3e3e-3e3e-3e3e3e3e3e3e");
    public static readonly Content NoteContent = new("This is a note");
    public static readonly NoteType NoteType = NoteType.AutoResponse;
}