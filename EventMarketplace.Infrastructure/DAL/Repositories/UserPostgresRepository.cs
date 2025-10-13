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

    public async Task<bool> CheckBusyEmail(string emailAddress)
    {
        var isBusy = await context.Users.FirstOrDefaultAsync(x => x.EmailAddress.Value.Equals(emailAddress));
        return isBusy != null;
    }
}