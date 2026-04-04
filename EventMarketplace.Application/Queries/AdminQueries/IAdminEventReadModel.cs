using EventMarketplace.Application.Response.EventResponse.AdminResponses;

namespace EventMarketplace.Application.Queries.AdminQueries;

public interface IAdminEventReadModel
{
    Task<AdminEventListResponse> GetAdminEventListAsync(GetAdminEventsQuery query, CancellationToken cancellationToken);
}