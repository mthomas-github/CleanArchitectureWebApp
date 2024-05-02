﻿using Asp.Versioning;
using ThirdPartyFreight.Application.Abstractions.Authentication;
using ThirdPartyFreight.Application.Abstractions.Caching;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Email;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Audits;
using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Domain.Documents;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Domain.Notes;
using ThirdPartyFreight.Domain.Sites;
using ThirdPartyFreight.Domain.Users;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Customer;
using ThirdPartyFreight.Domain.WorkflowTask;
using ThirdPartyFreight.Infrastructure.Authentication;
using ThirdPartyFreight.Infrastructure.Authorization;
using ThirdPartyFreight.Infrastructure.Caching;
using ThirdPartyFreight.Infrastructure.Clock;
using ThirdPartyFreight.Infrastructure.Data;
using ThirdPartyFreight.Infrastructure.DocuSign;
using ThirdPartyFreight.Infrastructure.Email;
using ThirdPartyFreight.Infrastructure.Repositories;
using AuthenticationOptions = ThirdPartyFreight.Infrastructure.Authentication.AuthenticationOptions;
using AuthenticationService = ThirdPartyFreight.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = ThirdPartyFreight.Application.Abstractions.Authentication.IAuthenticationService;

namespace ThirdPartyFreight.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        AddPersistence(services, configuration);
        AddAuthentication(services, configuration);
        AddAuthorization(services);
        AddCaching(services, configuration);
        AddHealthChecks(services, configuration);
        AddApiVersioning(services);
        AddDocuSign(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                  throw new ArgumentException("Invalid configuration parameter", nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IAgreementRepository, AgreementRepository>();

        services.AddScoped<ICarrierRepository, CarrierRepository>();

        services.AddScoped<IDocumentRepository, DocumentRepository>();

        services.AddScoped<ISiteRepository, SiteRepository>();

        services.AddScoped<INoteRepository, NoteRepository>();

        services.AddScoped<IEnvelopeRepository, EnvelopeRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuditRepository, AuditRepository>();

        services.AddScoped<IApprovalRepository, ApprovalRepository>();

        services.AddScoped<IWorkFlowTaskRepository, WorkFlowTaskRepository>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
            {
                KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            KeycloakOptions keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();

        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Cache") ??
                                  throw new ArgumentException("Invalid configuration parameter", nameof(configuration));


        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);


        services.AddScoped<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!)
            .AddRedis(configuration.GetConnectionString("Cache")!)
            .AddUrlGroup(new Uri(configuration["KeyCloak:BaseUrl"]!), HttpMethod.Get, "keycloak");
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddDocuSign(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDocuSignService, DocuSignService>();
        services.Configure<DocuSignOptions>(configuration.GetSection("DocuSign"));
    }


}
