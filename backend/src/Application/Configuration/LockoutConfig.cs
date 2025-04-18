namespace Application.Configuration;

public class LockoutConfig
{
    public int MaxFailedAttempts { get; set; } = 5;
    public int LockoutDurationMinutes { get; set; } = 60;
}