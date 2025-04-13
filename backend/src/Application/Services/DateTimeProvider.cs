using Application.Interfaces;

namespace Application.Services;

//todo: move to shared kernel layer 
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
