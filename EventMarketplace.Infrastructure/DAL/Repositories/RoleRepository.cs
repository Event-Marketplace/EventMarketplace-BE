using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class RoleRepository(EventMarketplaceDbContext context) : IRoleRepository
{
    public async Task<Role> GetRoleByEnumAsync(RoleType roleType)
        => await context.Roles.SingleOrDefaultAsync(x => x.RoleType == roleType);
}