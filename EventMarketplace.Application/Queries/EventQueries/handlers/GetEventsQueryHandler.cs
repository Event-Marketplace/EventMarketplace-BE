using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using MediatR;

namespace EventMarketplace.Application.Queries.EventQueries;

public sealed class GetEventsQueryHandler(IEventReadModel eventReadModel) : IRequestHandler<GetEventsQuery, EventListResponse>
{
    public async Task<EventListResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        return await eventReadModel.GetEventsList(request, cancellationToken);
    }
}