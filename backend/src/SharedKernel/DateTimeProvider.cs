using SharedKernel.Interfaces;

namespace SharedKernel;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
