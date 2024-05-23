using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Identity;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.MassTransit.Extensions;
using Elsa.Workflows.Management.Services;
using Medallion.Threading.Redis;
using Elsa.Workflows.Management.Compression;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
const bool runEfCoreMigrations = true;
ConfigurationManager configuration = builder.Configuration;
string sqlServerConnectionString = configuration.GetConnectionString("AppsDb")!;
string redisConnectionString = configuration.GetConnectionString("Redis")!;
IConfigurationSection identitySection = configuration.GetSection("Identity")!;
IConfigurationSection identityTokenSection = identitySection.GetSection("Tokens")!;
string rabbitMqConnectionString = configuration.GetConnectionString("RabbitMq")!;

builder.Services.AddElsa(elsa =>
{
    elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef =>
    {
        ef.UseSqlServer(sqlServerConnectionString!);
        ef.RunMigrations = runEfCoreMigrations;
        management.SetCompressionAlgorithm(nameof(Zstd));
        management.UseWorkflowInstances(feature =>
            feature.WorkflowInstanceStore = sp => sp.GetRequiredService<MemoryWorkflowInstanceStore>());
        management.UseMassTransitDispatcher();
    }));

    elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef =>
        {
            ef.UseSqlServer(sqlServerConnectionString!);
            ef.RunMigrations = runEfCoreMigrations;
            runtime.UseMassTransitDispatcher();
            runtime.WorkflowInboxCleanupOptions = options =>
                configuration.GetSection("Runtime:WorkflowInboxCleanup").Bind(options);
            runtime.WorkflowDispatcherOptions =
                options => configuration.GetSection("Runtime:WorkflowDispatcher").Bind(options);
            runtime.DistributedLockProvider = _ =>
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
                IDatabase database = connectionMultiplexer.GetDatabase();
                return new RedisDistributedSynchronizationProvider(database);
            };
        })
    );
    elsa.UseEnvironments(environments => environments.EnvironmentsOptions =
        options => configuration.GetSection("Environments").Bind(options));
    elsa.UseScheduling(scheduling => scheduling.UseQuartzScheduler());

    elsa.UseIdentity(identity =>
    {        
        
        
        identity.TokenOptions = options => options.SigningKey = "ssaf3b19e8c2047f6a98d7b2fb1d9c586fcf53e6873b92b7641d55a70c4a8db1e6"; // This key needs to be at least 256 bits long.
        identity.UseAdminUserProvider();
             // identity.UseConfigurationBasedUserProvider(options => identitySection.Bind(options));
             // identity.UseConfigurationBasedApplicationProvider(options => identitySection.Bind(options));
             // identity.UseConfigurationBasedRoleProvider(options => identitySection.Bind(options));

        // identity.UseEntityFrameworkCore(ef =>
        // {
        //     ef.UseSqlServer(sqlServerConnectionString!);
        //     ef.RunMigrations = runEfCoreMigrations;
        // });
    });

    elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

    elsa.UseWorkflowsApi();

    elsa.UseRealTimeWorkflows();

    elsa.UseJavaScript(options => options.AllowClrAccess = true);

    elsa.UseEmail(email => email.ConfigureOptions = options => configuration.GetSection("Smtp").Bind(options));
    elsa.AddSwagger();
    elsa.UseWebhooks(webhooks =>
        webhooks.WebhookOptions = options => builder.Configuration.GetSection("Webhooks").Bind(options));
    elsa.UseMassTransit(massTransit => massTransit.UseRabbitMq(rabbitMqConnectionString, rabbit =>
        rabbit.ConfigureServiceBus = bus =>
        {
            bus.PrefetchCount = 4;
            bus.Durable = true;
            bus.AutoDelete = false;
            bus.ConcurrentMessageLimit = 32;
            // etc.
        }));
});

builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("x-elsa-workflow-instance-id")));

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseCors();
app.MapHealthChecks("/");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseJsonSerializationErrorHandler();
app.UseWorkflows();
// Swagger API documentation.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseWorkflowsSignalRHubs();

app.Run();
