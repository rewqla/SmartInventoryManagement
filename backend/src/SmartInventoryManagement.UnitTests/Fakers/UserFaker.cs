namespace SmartInventoryManagement.Tests.Fakers;

public sealed class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(u => u.Id, f => f.Random.Guid());
        RuleFor(u => u.FullName, f => f.Name.FullName());
        RuleFor(u => u.Email, f => f.Internet.Email());
        RuleFor(u => u.PasswordHash, f => f.Random.Hash());
        RuleFor(u => u.Role, _ => new Role { Name = "User" });
    }
}