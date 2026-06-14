using EventMarketplace.Application.Queries;
using EventMarketplace.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class UserReadModel(EventMarketplaceDbContext context) : IUserReadModel
{
    public async Task<List<User>> GetUsersAsync()
    {
        return await context.Users.ToListAsync();
    }
}