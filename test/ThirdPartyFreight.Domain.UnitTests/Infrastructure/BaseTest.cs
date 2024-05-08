using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.UnitTests.Infrastructure;

public abstract class BaseTest
{
    protected static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent
    {
        T domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault() ?? throw new Exception($"{typeof(T).Name} was not published.");

        return domainEvent;
    }
}
