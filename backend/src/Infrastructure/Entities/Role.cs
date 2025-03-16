namespace Infrastructure.Entities;

//todo: move to enum of roles
public sealed class Role : BaseEntity
{
    public string Name { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}