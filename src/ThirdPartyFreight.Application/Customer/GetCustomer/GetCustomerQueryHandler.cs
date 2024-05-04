using System.Data;
using Dapper;
using Newtonsoft.Json;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Customer;

namespace ThirdPartyFreight.Application.Customer.GetCustomer;

internal sealed class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerSiteResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomerQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<CustomerSiteResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string storedProcedureName = "TPF_GetCustomerActiveSites";

        string? jsonResponse = await connection.QueryFirstOrDefaultAsync<string>(storedProcedureName,
            new { request.CustomerNumber },
            commandType: CommandType.StoredProcedure);

        CustomerSiteResponse? result = JsonConvert.DeserializeObject<CustomerSiteResponse>(jsonResponse ?? string.Empty);

        return result ?? Result.Failure<CustomerSiteResponse>(CustomerErrors.NotFound);
    }
}
