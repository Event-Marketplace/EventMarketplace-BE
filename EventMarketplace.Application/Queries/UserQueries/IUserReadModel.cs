using EventMarketplace.Application.Responses.UserResponse;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Application.Queries;

public interface IUserReadModel
{
    Task<GetUsersListResponse> GetUsersAsync(GetUsersQuery request, CancellationToken cancellationToken);
}