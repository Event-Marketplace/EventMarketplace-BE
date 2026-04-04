using EventMarketplace.Application.Response.EventResponse;
using MediatR;

namespace EventMarketplace.Application.Queries.EventQueries;

public class GetOrganizerEventsQueryHandler(IEventReadModel eventReadModel) : IRequestHandler<GetOrganizerEventsQuery, EventListResponse>
{
    public async Task<EventListResponse> Handle(GetOrganizerEventsQuery request, CancellationToken cancellationToken)
    {
        return await eventReadModel.GetOrganizerEventsList(request, cancellationToken);
    }
}