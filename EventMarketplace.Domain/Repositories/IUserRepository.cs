using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> CheckBusyEmail(string emailAddress);
}