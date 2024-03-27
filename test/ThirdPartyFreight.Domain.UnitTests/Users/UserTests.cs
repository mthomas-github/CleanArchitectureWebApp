using ThirdPartyFreight.Domain.Users;
using ThirdPartyFreight.Domain.Users.Events;
using FluentAssertions;
using ThirdPartyFreight.Domain.UnitTests.Infrastructure;

namespace ThirdPartyFreight.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Arrange See UserData.cs

        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        
        // Assert
        user.FirstName.Should().Be(UserData.FirstName);
        user.LastName.Should().Be(UserData.LastName);
        user.Email.Should().Be(UserData.Email);
    }

    [Fact]
    public void Create_Should_RaiseUserCreatedDomainEvent()
    {
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        UserCreatedDomainEvent domainEvents = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        domainEvents.UserId.Should().Be(user.Id);

    }

    [Fact]
    public void Create_Should_AddRegisteredRoleToUser()
    {
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        user.Roles.Should().Contain(Role.Registered);
    }
}
