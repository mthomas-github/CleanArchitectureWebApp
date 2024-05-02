using System.Data;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Application.Customer.GetCustomers;

internal sealed class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, IReadOnlyList<CustomerResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<CustomerResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                            CustomerNumber,
                            CustomerName
                           FROM
                            View_TPFCustomerMaster;
                           """;

        IEnumerable<CustomerResponse> customers = await connection.QueryAsync<CustomerResponse>(sql);

        return customers.ToList();
    }
}
