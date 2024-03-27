﻿using ThirdPartyFreight.Domain.Users;

namespace ThirdPartyFreight.Domain.UnitTests.Users;

internal static class UserData
{
    public static readonly FirstName FirstName = new("First");
    public static readonly LastName LastName = new("Last");
    public static readonly Email Email = new("test@test.com");
}