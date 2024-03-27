using System.Reflection;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Infrastructure;

namespace ThirdPartyFreight.ArchitectureTests.Infrastructure;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(IBaseCommand).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
