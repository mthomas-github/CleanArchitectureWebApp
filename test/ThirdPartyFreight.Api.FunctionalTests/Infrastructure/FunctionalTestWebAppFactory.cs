using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Infrastructure;
using ThirdPartyFreight.Infrastructure.Authentication;
using ThirdPartyFreight.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;
using DotNet.Testcontainers.Builders;
using Testcontainers.Keycloak;
using Testcontainers.MsSql;
using Testcontainers.Redis;
using ThirdPartyFreight.Api.FunctionalTests.Users;

namespace ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
        .WithImage("quay.io/keycloak/keycloak:latest")
        .WithResourceMapping(
            new FileInfo(".files/csd-tpf-realm-export.json"),
            new FileInfo("/opt/keycloak/data/import/realm.json"))
        .WithCommand("--import-realm")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            string connectionString = $"{_dbContainer.GetConnectionString()};Pooling=False";

            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(connectionString));

            services.RemoveAll(typeof(ISqlConnectionFactory));

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new SqlConnectionFactory(connectionString));

            services.Configure<RedisCacheOptions>(redisCacheOptions =>
                redisCacheOptions.Configuration = _redisContainer.GetConnectionString());

            string? keycloakAddress = _keycloakContainer.GetBaseAddress();

            services.Configure<KeycloakOptions>(o =>
            {
                o.AdminUrl = $"{keycloakAddress}admin/realms/csd-tpf/";
                o.TokenUrl = $"{keycloakAddress}realms/csd-tpf/protocol/openid-connect/token";
            });

            services.Configure<AuthenticationOptions>(o =>
            {
                o.Issuer = $"{keycloakAddress}realms/csd-tpf/";
                o.MetadataUrl = $"{keycloakAddress}realms/csd-tpf/.well-known/openid-configuration";
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _keycloakContainer.StartAsync();

        await InitializeTestUserAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
        await _keycloakContainer.StopAsync();
    }

    private async Task InitializeTestUserAsync()
    {
        try
        {
            using HttpClient httpClient = CreateClient();

            await httpClient.PostAsJsonAsync("api/v1/users/register", UserData.RegisterTestUserRequest);
        }
        catch
        {
            // Do nothing.
        }
    }
}
