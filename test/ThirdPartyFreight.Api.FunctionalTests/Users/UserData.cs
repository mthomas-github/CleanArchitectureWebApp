using ThirdPartyFreight.Api.Controllers.Users;

namespace ThirdPartyFreight.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");
}
