
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public sealed class GetEventsQueryHandler(EventMarketplaceDbContext context) : IRequestHandler<GetEventsQuery, EventListResponse>
{
    public async Task<EventListResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = context.Events
            .Where(x => x.IsActive)
            .FilterEvents(request)
            .OrderBy(x => x.DurationOfTheEvent.StartEvent);
            
        var paginatedResult = await events
            .PaginationEvents(request)
            .ToListAsync(cancellationToken: cancellationToken);

        return new EventListResponse()
        {
            Events = paginatedResult.Select(x => new EventResponse()
            {
                Id = x.Id,
                Description = x.Description,
                Price = x.Price,
                Title = x.Title,
                AvailableTickets = x.AvailableTickets,
                StartDate = x.DurationOfTheEvent.StartEvent,
                EndDate = x.DurationOfTheEvent.EndEvent,
                IsActive = x.IsActive,
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreateAt
            }).ToList(),
            TotalCount = events.Count()
        };
    }
}