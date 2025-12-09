using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using MediatR;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class GetEventStatusesQueryHandler : IRequestHandler<GetEventStatusesQuery, List<EventStatusResponse>>
{
    public async Task<List<EventStatusResponse>> Handle(GetEventStatusesQuery request, CancellationToken cancellationToken)
    {
        var statuses = Enum.GetValues(typeof(EventStatus))
            .Cast<EventStatus>()
            .Select(x => new
            {
                index = (int)x,
                name = x.ToString(),
                displayName = x.GetDisplayName()
            });

        return statuses.Select(x => new EventStatusResponse()
        {
            StatusIndex = x.index,
            StatusDisplayName = x.displayName,
            StatusName = x.name
        }).ToList();
    }
}