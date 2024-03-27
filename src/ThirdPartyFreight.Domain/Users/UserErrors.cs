using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.Found",
        "The user with the specified identifier was not found");

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");

    public static readonly Error UnableToCreate = new(
        "User.UnableCreate",
        "There was a problem with creating a user.");

}
