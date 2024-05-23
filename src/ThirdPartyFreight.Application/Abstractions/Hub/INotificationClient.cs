namespace ThirdPartyFreight.Application.Abstractions.Hub;

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}
