using EventMarketplace.Application.Response.EventResponse;

namespace EventMarketplace.Application.Queries;

public interface IEventReadModel
{
    Task<EventListResponse> GetEventsList(GetEventsQuery query, CancellationToken cancellationToken);
    Task<EventListResponse> GetOrganizerEventsList(GetOrganizerEventsQuery query, CancellationToken cancellationToken);
}