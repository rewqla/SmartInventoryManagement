namespace Infrastructure.Entities;

public sealed class User : BaseEntity
{
    //todo: rename to full name
    public string Name { get; set; }

    //todo: rename to phone number
    public string Phone { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>(); 
    
    //todo: add created at variable
}