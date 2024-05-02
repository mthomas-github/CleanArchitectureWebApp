using System.Data;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Customer;

namespace ThirdPartyFreight.Application.Customer.GetCustomer;

internal sealed class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomerQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<Result<CustomerResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                             CustomerNumber,
                             CustomerName
                           FROM
                             View_TPFCustomerMaster
                           WHERE
                             CustomerNumber = @CustomerNumber;
                           """;

        CustomerResponse? result = await connection.QueryFirstOrDefaultAsync<CustomerResponse>(sql, new { request.CustomerNumber });

        return result ?? Result.Failure<CustomerResponse>(CustomerErrors.NotFound);
    }
}
