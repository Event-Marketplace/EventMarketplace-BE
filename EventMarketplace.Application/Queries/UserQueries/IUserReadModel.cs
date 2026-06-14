using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Application.Queries;

public interface IUserReadModel
{
    Task<List<User>> GetUsersAsync();
}