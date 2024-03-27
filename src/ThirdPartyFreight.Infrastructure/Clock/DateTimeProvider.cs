using ThirdPartyFreight.Application.Abstractions.Clock;

namespace ThirdPartyFreight.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    
}