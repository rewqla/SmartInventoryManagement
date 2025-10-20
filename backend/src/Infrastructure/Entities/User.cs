namespace Infrastructure.Entities;

public sealed class User : BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<InventoryLog> InventoryLogs { get; set; } = [];
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockoutEnd { get; set; }
    public DateTime CreatedAt { get; set; }
}
