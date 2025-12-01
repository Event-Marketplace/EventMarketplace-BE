using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Domain.Entities;

public class Role : BaseEntity
{
    public string DisplayName { get; set; }
    public RoleType RoleType { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}