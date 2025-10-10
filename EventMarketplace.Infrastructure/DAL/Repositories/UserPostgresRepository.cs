using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class UserPostgresRepository(EventMarketplaceDbContext context) : IUserRepository
{
    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
    }
}