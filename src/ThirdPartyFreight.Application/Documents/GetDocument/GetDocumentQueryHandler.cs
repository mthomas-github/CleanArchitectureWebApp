using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Documents;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Documents.GetDocument;

internal sealed class GetDocumentQueryHandler : IQueryHandler<GetDocumentQuery, DocumentResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetDocumentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<DocumentResponse>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            d.Id DocumentId,
                            d.DocumentDetails_DocumentName DocumentName,
                            d.DocumentDetails_DocumentData DocumentData,
                            d.DocumentDetails_Type DocumentType
                           FROM
                            TPF_Documents d
                           WHERE
                            d.Id = @DocumentId;
                           """;

        DocumentResponse? result = await connection.QueryFirstOrDefaultAsync<DocumentResponse>(sql, new { request.DocumentId });

        return result ?? Result.Failure<DocumentResponse>(DocumentErrors.NotFound);
    }
}
