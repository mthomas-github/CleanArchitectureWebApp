using System.Data;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Carriers;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Carriers.GetCarrier;

internal sealed class GetCarrierQueryHandler : IQueryHandler<GetCarrierQuery, CarrierResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCarrierQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<CarrierResponse>> Handle(GetCarrierQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            c.Id CarrierId,
                            c.CarrierInfo_CarrierName CarrierName,
                            c.CarrierInfo_CarrierAccount CarrierAccount,
                            c.CarrierInfo_CarrierType CarrierType
                           FROM
                            TPF_Carriers c
                           WHERE
                            c.Id = @CarrierId;
                           """;
        CarrierResponse? result = await connection.QueryFirstOrDefaultAsync<CarrierResponse>(sql, new { request.CarrierId });

        return result ?? Result.Failure<CarrierResponse>(CarrierErrors.NotFound);
                           
    }
}
