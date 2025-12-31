using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class UserPostgresRepository(EventMarketplaceDbContext context) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.Users.SingleOrDefaultAsync(x => x.EmailAddress.Value.Equals(email));
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
        => await context.Users.SingleOrDefaultAsync(x => x.Id == userId);

    public async Task<bool> CheckBusyEmail(string emailAddress)
    {
        var isBusy = await context.Users.FirstOrDefaultAsync(x => x.EmailAddress.Value.Equals(emailAddress));
        return isBusy != null;
    }

    public List<string> GetUserRoles(Guid userId)
        =>  context.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.Role.RoleType.ToString())
            .ToList();
 
}