using Microsoft.Extensions.Options;
using Quartz;
using ThirdPartyFreight.Infrastructure.OutBox;

namespace ThirdPartyFreight.Infrastructure.DocuSign.BackgroundJobs;

internal sealed class ProcessStatusUpdateJobSetup(IOptions<OutboxOptions> outboxOptions) 
    : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessStatusUpdateJob);

        options
            .AddJob<ProcessStatusUpdateJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}
