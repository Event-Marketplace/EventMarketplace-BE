namespace EventMarketplace.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public static UserRole Create(User user, Role role)
    {
        return new UserRole()
        {
            UserId = user.Id,
            RoleId = role.Id,
            User = user,
            Role = role
        };
    }
}