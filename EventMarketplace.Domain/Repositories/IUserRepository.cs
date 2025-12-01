using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<bool> CheckBusyEmail(string emailAddress);
}