using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;
