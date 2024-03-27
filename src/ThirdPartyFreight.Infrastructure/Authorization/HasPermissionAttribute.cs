using Microsoft.AspNetCore.Authorization;

namespace ThirdPartyFreight.Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
