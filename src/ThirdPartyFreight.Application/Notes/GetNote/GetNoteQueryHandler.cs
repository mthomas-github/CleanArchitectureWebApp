using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Notes;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Notes.GetNote;

internal sealed class GetNoteQueryHandler : IQueryHandler<GetNoteQuery, NoteResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    public GetNoteQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<NoteResponse>> Handle(GetNoteQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            n.Id NoteId,
                            n.Content NoteContent,
                            n.NoteType,
                            n.CreatedOnUtc CreateAt
                           FROM
                            TPF_Notes n
                           WHERE
                            n.Id = @NoteId;
                           """;
        NoteResponse? result = await connection.QueryFirstOrDefaultAsync<NoteResponse>(sql, new { request.NoteId });

        return result ?? Result.Failure<NoteResponse>(NoteErrors.NotFound);
    }
}
