using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;

namespace ThirdPartyFreight.Infrastructure.Hubs;

public class ServerTimeNotifier(
    ILogger<ServerTimeNotifier> logger,
    IHubContext<NotificationHub, INotificationClient> hubContext)
    : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Period);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            DateTime dateTime = DateTime.Now;
            logger.LogInformation("Executing {Service} {Time}", nameof(ServerTimeNotifier), dateTime);
            await hubContext.Clients.All.ReceiveNotification($"Server Time = {dateTime}");
        }

    }
}
