using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Sites;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Sites.GetSite;

internal sealed class GetSiteQueryHandler : IQueryHandler<GetSiteQuery, SiteResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetSiteQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<SiteResponse>> Handle(GetSiteQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            s.Id SiteId,
                            s.SiteNumber,
                            s.SiteAddress_Street Street,
                            s.SiteAddress_City City,
                            s.SiteAddress_State [State],
                            s.SiteAddress_ZipCode ZipCode
                           FROM
                            TPF_Sites s
                           WHERE
                            s.Id = @SiteId;
                           """;

        SiteResponse? result = await connection.QueryFirstOrDefaultAsync<SiteResponse>(sql, new { request.SiteId });

        return result ?? Result.Failure<SiteResponse>(SiteErrors.NotFound);
    }
}
