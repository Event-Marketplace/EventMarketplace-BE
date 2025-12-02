using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role> GetRoleByEnumAsync(RoleType roleType);
}