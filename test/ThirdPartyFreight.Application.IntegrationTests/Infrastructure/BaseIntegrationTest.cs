using ThirdPartyFreight.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ThirdPartyFreight.Application.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly ISender Sender;
    protected readonly ApplicationDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
#pragma warning disable CA2000
        IServiceScope scope = factory.Services.CreateScope();
#pragma warning restore CA2000

        Sender = scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
