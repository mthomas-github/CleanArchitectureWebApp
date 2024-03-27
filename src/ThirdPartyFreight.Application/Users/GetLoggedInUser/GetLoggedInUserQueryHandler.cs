using System.Data;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Notes;
using ThirdPartyFreight.Domain.Sites;
using ThirdPartyFreight.Domain.Users;
using Dapper;
using ThirdPartyFreight.Application.Abstractions.Authentication;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;

    public GetLoggedInUserQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IUserContext userContext)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(
        GetLoggedInUserQuery request,
        CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                Id,
                FirstName,
                LastName,
                Email
            FROM TPF_Users
            WHERE IdentityId = @IdentityId
            """;

        UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            new
            {
                _userContext.IdentityId
            });

        return user ?? Result.Failure<UserResponse>(UserErrors.NotFound);
    }
}
