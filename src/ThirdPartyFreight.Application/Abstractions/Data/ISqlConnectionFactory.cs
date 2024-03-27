using System.Data;

namespace ThirdPartyFreight.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}