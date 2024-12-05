namespace Infrastructure.Entities;

public sealed class Role : BaseEntity
{
    public string Name { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}
