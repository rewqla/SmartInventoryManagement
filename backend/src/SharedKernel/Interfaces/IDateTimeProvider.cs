namespace SharedKernel.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
