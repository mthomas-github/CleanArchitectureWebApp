using ThirdPartyFreight.Domain.Users;

namespace ThirdPartyFreight.Infrastructure.Authorization;

internal sealed class UserRolesResponse
{
    public Guid UserId { get; init; }

    public List<Role> Roles { get; init; } = [];
}
